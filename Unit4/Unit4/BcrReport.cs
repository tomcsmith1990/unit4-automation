using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Data;
using System.Diagnostics;
using System.Collections.Concurrent;
using Unit4.Interfaces;

namespace Unit4
{
    internal class BcrReport
    {
        private enum Tier { Tier3, Tier4, CostCentre };

        private readonly BCRLineBuilder _builder = new BCRLineBuilder();
        private readonly ILogging _log;
        private readonly IUnit4EngineFactory _factory;

        public BcrReport(IUnit4EngineFactory factory, ILogging log)
        {
            _factory = factory;
            _log = log;
        }

        public IEnumerable<BCRLine> RunBCR(IEnumerable<IGrouping<string, CostCentre>> hierarchy)
        {
            var reportsToRun = hierarchy.Select(x => new Report() { Tier = Tier.Tier3, Hierarchy = x });
            var bag = new ConcurrentBag<BCRLine>();

            Parallel.ForEach(reportsToRun, new ParallelOptions { MaxDegreeOfParallelism = 3 }, t =>
            {
                var bcrLines = RunBCR(t);
                foreach (var line in bcrLines)
                {
                    bag.Add(line);
                }
            });

            return bag;
        }

        private class Report
        {
            public Tier Tier { get; set; }
            public IGrouping<string, CostCentre> Hierarchy { get; set; }

            public string Parameter { get { return Hierarchy.Key; } }

            public bool ShouldFallBack { get { return (Tier == Tier.Tier3 || Tier == Tier.Tier4) && Hierarchy.Any(); } }

            public IEnumerable<Report> FallbackReports()
            {
                var fallbackGroups = Hierarchy.GroupBy(FallBackGroupingFunction(Tier), x => x);

                return fallbackGroups.Select(x => new Report() { Tier = FallBackTier(Tier), Hierarchy = x });
            }

            private Tier FallBackTier(Tier current)
            {
                switch (current)
                {
                    case Tier.Tier3: return Tier.Tier4;
                    case Tier.Tier4: return Tier.CostCentre;
                    default: throw new InvalidOperationException("Should not fall back");
                }
            }

            private Func<CostCentre, string> FallBackGroupingFunction(Tier current)
            {
                switch (current)
                {
                    case Tier.Tier3: return x => x.Tier4;
                    case Tier.Tier4: return x => x.Code;
                    default: throw new InvalidOperationException("Should not fall back");
                }
            }
        }

        private IEnumerable<BCRLine> RunBCR(Report report)
        {
            string value = report.Parameter;
            try
            {
                var bcr = RunReport(report.Tier, value);
                _log.Info(string.Format("Got BCR for {0}", value));

                return _builder.Build(bcr).ToList();
            }
            catch (Exception e)
            {
                _log.Error(string.Format("Error getting BCR for {0}", value));
                _log.Error(e);

                if (report.ShouldFallBack) 
                {
                    var fallbackReports = report.FallbackReports();
                    _log.Info(string.Format("Falling back to {0}: ", string.Join(",", fallbackReports.Select(x => x.Parameter).ToArray())));
                    return fallbackReports.SelectMany(RunBCR).ToList();
                }

                return Enumerable.Empty<BCRLine>();
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