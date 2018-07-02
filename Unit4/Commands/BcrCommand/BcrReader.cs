using System.IO;
using Unit4.Automation.Interfaces;
using Unit4.Automation.ReportEngine;
using Unit4.Automation.Model;
using System.Collections.Generic;
using System.Linq;

namespace Unit4.Automation.Commands.BcrCommand
{
    internal class BcrReader : IBcrReader
    {
        private readonly ILogging _log;
        private readonly CostCentreHierarchy _hierarchy;
        private readonly ProgramConfig _config;
        private readonly IFile<Bcr> _bcrFile;
        private readonly IUnit4EngineFactory _factory;

        public BcrReader(ILogging log, BcrOptions options, ProgramConfig config, IFile<Bcr> bcrFile, IFile<SerializableCostCentreList> costCentreFile, IUnit4EngineFactory factory, ICostCentresProvider provider)
        {
            _log = log;
            _bcrFile = bcrFile;
            _factory = factory;

            var costCentreList = 
                new Cache<SerializableCostCentreList>(
                        () => provider.GetCostCentres(), 
                        costCentreFile);
            _hierarchy = new CostCentreHierarchy(costCentreList, options);
            _config = config;
        }

        public Bcr Read()
        {
            var tier3Hierarchy = _hierarchy.GetHierarchyByTier3();

            var cachedLines = GetCachedLines();
            var cachedTier3s = cachedLines.Select(x => x.CostCentre.Tier3).Distinct();

            var hierarchyToFetch = tier3Hierarchy.Where(x => !cachedTier3s.Contains(x.Key));

            if (!hierarchyToFetch.Any())
            {
                return new Bcr(cachedLines);
            }

            var fetchedLines = new BcrReport(_factory, _log).RunBcr(hierarchyToFetch).Lines;

            var allLines = fetchedLines.Union(cachedLines);

            return new Bcr(allLines);
        }

        private IEnumerable<BcrLine> GetCachedLines()
        {
            return new Cache<Bcr>(() => new Bcr(Enumerable.Empty<BcrLine>()), _bcrFile).Fetch().Lines;
        }
    }
}