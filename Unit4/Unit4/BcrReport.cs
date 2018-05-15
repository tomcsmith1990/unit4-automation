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
        private enum Tier { Tier3, Tier4 };

        private readonly BCRLineBuilder _builder = new BCRLineBuilder();
        private readonly ILogging _log;
        private readonly IUnit4EngineFactory _factory;

        public BcrReport(IUnit4EngineFactory factory, ILogging log)
        {
            _factory = factory;
            _log = log;
        }

        public IEnumerable<BCRLine> RunBCR(IGrouping<string, CostCentre> hierarchy)
        {
            return RunBCR(Tier.Tier3, hierarchy.Key, hierarchy.Select(x => x.Tier4).Distinct());
        }

        private IEnumerable<BCRLine> RunBCR(Tier tier, string value, IEnumerable<string> fallback)
        {
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

                if (fallback.Any()) 
                {
                    _log.Info(string.Format("Falling back to {0}: ", string.Join(",", fallback.ToArray())));
                    return fallback.SelectMany(x => RunBCR(Tier.Tier4, x, Enumerable.Empty<string>())).ToList();
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
                    resql = string.Format(Resql.BcrByTier3, value);
                    break;
                case Tier.Tier4:
                    resql = string.Format(Resql.BcrByTier4, value);
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