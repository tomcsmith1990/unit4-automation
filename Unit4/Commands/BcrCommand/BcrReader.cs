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
        private readonly IFile<Bcr> _bcrFile;
        private readonly IUnit4EngineFactory _factory;

        public BcrReader(ILogging log, BcrOptions options, ProgramConfig config, IFile<Bcr> bcrFile, IFile<SerializableCostCentreList> costCentreFile, IUnit4EngineFactory factory)
        {
            _log = log;
            _bcrFile = bcrFile;
            _factory = factory;

            var costCentreList = 
                new Cache<SerializableCostCentreList>(
                        () => new CostCentresProvider(config, factory).GetCostCentres(), 
                        costCentreFile);
            _hierarchy = new CostCentreHierarchy(costCentreList, options);
            _config = config;
        }

        public Bcr Read()
        {
            var tier3Hierarchy = _hierarchy.GetHierarchyByTier3();

            var bcrReport = 
                new Cache<Bcr>(
                    () => new BcrReport(_factory, _log).RunBcr(tier3Hierarchy), 
                    _bcrFile);

            return bcrReport.Fetch();
        }
    }
}