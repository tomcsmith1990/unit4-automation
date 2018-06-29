using System.IO;
using Unit4.Automation.Interfaces;
using Unit4.Automation.ReportEngine;
using Unit4.Automation.Model;

namespace Unit4.Automation.Commands.BcrCommand
{
    internal class BcrReader : IBcrReader
    {
        private readonly ILogging _log;
        private readonly CostCentreHierarchy _hierarchy;
        private readonly ProgramConfig _config;

        public BcrReader(ILogging log, ProgramConfig config)
        {
            _log = log;

            var costCentreList = 
                new Cache<SerializableCostCentreList>(
                        () => new CostCentresProvider(config).GetCostCentres(), 
                        new JsonFile<SerializableCostCentreList>(Path.Combine(Directory.GetCurrentDirectory(), "cache", "costCentres.json")));
            _hierarchy = new CostCentreHierarchy(costCentreList);
            _config = config;
        }

        public Bcr Read()
        {
            var tier3Hierarchy = _hierarchy.GetHierarchyByTier3();

            var factory = new Unit4EngineFactory(_config);
            var bcrReport = 
                new Cache<Bcr>(
                    () => new BcrReport(factory, _log).RunBcr(tier3Hierarchy), 
                    new JsonFile<Bcr>(Path.Combine(Directory.GetCurrentDirectory(), "cache", "bcr.json")));

            return bcrReport.Fetch();
        }
    }
}