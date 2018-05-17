using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Data;
using System.Diagnostics;
using System.Collections.Concurrent;
using Unit4.Interfaces;
using Unit4.Automation.Model;

namespace Unit4
{
    internal class BcrReport
    {
        public enum Tier { Tier3, Tier4, CostCentre };

        private readonly BCRLineBuilder _builder = new BCRLineBuilder();
        private readonly ILogging _log;
        private readonly IUnit4EngineFactory _factory;

        public BcrReport(IUnit4EngineFactory factory, ILogging log)
        {
            _factory = factory;
            _log = log;
        }

        public IEnumerable<BcrLine> RunBCR(IEnumerable<IGrouping<string, CostCentre>> hierarchy)
        {
            var reportsToRun = hierarchy.Select(x => new Report() { Tier = Tier.Tier3, Hierarchy = x });
            
            return RunBCR(reportsToRun).ToList();
        }

        private IEnumerable<BcrLine> RunBCR(IEnumerable<Report> reports)
        {
            var bag = new ConcurrentBag<BcrLine>();

            var extraReportsToRun = new ConcurrentBag<Report>();

            Parallel.ForEach(reports, new ParallelOptions { MaxDegreeOfParallelism = 3 }, t =>
            {
                try
                {
                    var bcrLines = RunBCR(t);
                    foreach (var line in bcrLines)
                    {
                        bag.Add(line);
                    }
                }
                catch (Exception)
                {
                    if (t.ShouldFallBack)
                    {
                        var fallbackReports = t.FallbackReports().ToList();
                        _log.Info(string.Format("Error getting BCR for {0}. Will fallback to {1}:{2}", t.Parameter, string.Join(Environment.NewLine, fallbackReports.Select(x => x.Parameter).ToArray()), Environment.NewLine));
                        fallbackReports.ForEach(r => extraReportsToRun.Add(r));
                    }
                }
            });

            if (extraReportsToRun.Any())
            {
                return bag.Concat(RunBCR(extraReportsToRun));
            }

            return bag;
        }

        private IEnumerable<BcrLine> RunBCR(Report report)
        {
            string value = report.Parameter;
            try
            {
                var bcr = RunReport(report.Tier, value);
                _log.Info(string.Format("Got BCR for {0}, contains {1} rows", value, bcr.Tables[0].Rows.Count));

                return _builder.Build(bcr).ToList();
            }
            catch (Exception e)
            {
                _log.Error(string.Format("Error getting BCR for {0}", value));
                _log.Error(e);

                throw;
            }
        }

        private DataSet RunReport(Tier tier, string value)
        {
            string resql;
            switch (tier)
            {
                case Tier.Tier3:
                    resql = Resql.Bcr(tier3: value);
                    break;
                case Tier.Tier4:
                    resql = Resql.Bcr(tier4: value);
                    break;
                case Tier.CostCentre:
                    resql = Resql.Bcr(costCentre: value);
                    break;
                default:
                    throw new InvalidOperationException("Cannot run a report for this tier") ;
            }

            return RunReport(resql);
        }

        private DataSet RunReport(string resql)
        {
            var engine = _factory.Create();

            return engine.RunReport(resql);
        }
    }
}