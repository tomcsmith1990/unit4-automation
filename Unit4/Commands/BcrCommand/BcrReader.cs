using System.Collections.Generic;
using System.Linq;
using Unit4.Automation.Interfaces;
using Unit4.Automation.Model;

namespace Unit4.Automation.Commands.BcrCommand
{
    internal class BcrReader : IBcrReader
    {
        private readonly IFile<Bcr> _bcrFile;
        private readonly IUnit4EngineFactory _factory;
        private readonly CostCentreHierarchy _hierarchy;
        private readonly ILogging _log;
        private readonly bool _updateCache;

        public BcrReader(ILogging log, BcrOptions options, IFile<Bcr> bcrFile, IFile<SerializableCostCentreList> costCentreFile, IUnit4EngineFactory factory, ICostCentresProvider provider)
        {
            _log = log;
            _bcrFile = bcrFile;
            _factory = factory;
            _updateCache = options.UpdateCache;

            var costCentreList =
                new Cache<SerializableCostCentreList>(
                    () => provider.GetCostCentres(),
                    costCentreFile);
            _hierarchy = new CostCentreHierarchy(costCentreList, options);
        }

        public Bcr Read()
        {
            var tier3Hierarchy = _hierarchy.GetHierarchyByTier3();

            var cachedLines = GetCachedLines();
            var cachedCostCentres = cachedLines.Select(x => x.CostCentre.Code).Distinct();

            var hierarchyToFetch = _updateCache
                ? tier3Hierarchy
                : tier3Hierarchy.Where(x => x.Any(y => !cachedCostCentres.Contains(y.Code)));

            if (!hierarchyToFetch.Any())
            {
                return new Bcr(cachedLines);
            }

            var fetchedLines = new BcrReport(_factory, _log).RunBcr(hierarchyToFetch).Lines;

            var fetchedCostCentres = fetchedLines.Select(x => x.CostCentre.Code).Distinct();
            var cachedLinesNotUpdated = cachedLines.Where(x => !fetchedCostCentres.Contains(x.CostCentre.Code));

            var bcr = new Bcr(fetchedLines.Union(cachedLinesNotUpdated));
            _bcrFile.Write(bcr);
            return bcr;
        }

        private IEnumerable<BcrLine> GetCachedLines()
        {
            return new Cache<Bcr>(() => new Bcr(Enumerable.Empty<BcrLine>()), _bcrFile).Fetch().Lines;
        }
    }
}