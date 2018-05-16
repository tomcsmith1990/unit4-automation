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
            var bag = new ConcurrentBag<BCRLine>();

            Parallel.ForEach(hierarchy, new ParallelOptions { MaxDegreeOfParallelism = 3 }, t =>
            {
                var bcrLines = RunBCR(Tier.Tier3, t);
                foreach (var line in bcrLines)
                {
                    bag.Add(line);
                }
            });

            return bag;
        }

        private IEnumerable<BCRLine> RunBCR(Tier tier, IGrouping<string, CostCentre> hierarchy)
        {
            string value = hierarchy.Key;
            try
            {
                var bcr = RunReport(tier, value);
                _log.Info(string.Format("Got BCR for {0}", value));

                return _builder.Build(bcr).ToList();
            }
            catch (Exception e)
            {
                _log.Error(string.Format("Error getting BCR for {0}", value));
                _log.Error(e);

                if (ShouldFallBack(tier) && hierarchy.Any()) 
                {
                    var fallbackGroups = hierarchy.GroupBy(FallBackGroupingFunction(tier), x => x);
                    _log.Info(string.Format("Falling back to {0}: ", string.Join(",", fallbackGroups.Select(x => x.Key).ToArray())));

                    return fallbackGroups.SelectMany(x => RunBCR(FallBackTier(tier), x)).ToList();
                }

                return Enumerable.Empty<BCRLine>();
            }
        }

        private bool ShouldFallBack(Tier current)
        {
            return current == Tier.Tier3 || current == Tier.Tier4;
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