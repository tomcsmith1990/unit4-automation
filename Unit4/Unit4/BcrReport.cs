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
        private readonly BCRLineBuilder _builder = new BCRLineBuilder();
        private readonly Logging _log;
        private readonly IUnit4EngineFactory _factory;

        public BcrReport(IUnit4EngineFactory factory, Logging log)
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

                return _builder.Build(bcr);
            }
            catch (Exception e)
            {
                _log.Error(string.Format("Error getting BCR for {0}, falling back to tier 4", hierarchy.Key));
                _log.Error(e);
                return hierarchy.Select(x => x.Tier4).Distinct().SelectMany(x => RunBCRTier4(x));
            }
        }

        private DataSet RunBCRTier3(string tier3)
        {
            return RunReport(string.Format(Resql.BcrByTier3, tier3));
        }

        private IEnumerable<BCRLine> RunBCRTier4(string tier4)
        {
            try
            {
                var bcr = RunReport(string.Format(Resql.BcrByTier4, tier4));
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

        private DataSet RunReport(string resql)
        {
            var engine = _factory.Create();

            return engine.RunReport(resql);
        }
    }
}