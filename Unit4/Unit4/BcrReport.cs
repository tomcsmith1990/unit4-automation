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
            try
            {
                var bcr = RunBCRTier3(hierarchy.Key);
                _log.Info(string.Format("Got BCR for {0}", hierarchy.Key));

                return _builder.Build(bcr).ToList();
            }
            catch (Exception e)
            {
                _log.Error(string.Format("Error getting BCR for {0}, falling back to tier 4", hierarchy.Key));
                _log.Error(e);
                return hierarchy.Select(x => x.Tier4).Distinct().SelectMany(x => RunBCRTier4(x)).ToList();
            }
        }

        private DataSet RunBCRTier3(string tier3)
        {
            return RunReport(Tier.Tier3, tier3);
        }

        private IEnumerable<BCRLine> RunBCRTier4(string tier4)
        {
            try
            {
                var bcr = RunReport(Tier.Tier4, tier4);
                _log.Info(string.Format("Got BCR for {0}", tier4));

                return _builder.Build(bcr);
            }
            catch (Exception e)
            {
                _log.Error(string.Format("Error getting BCR for {0}", tier4));
                _log.Error(e);
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