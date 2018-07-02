using System.Collections.Generic;
using System.Linq;
using Unit4.Automation.Interfaces;
using Unit4.Automation.Model;
using Unit4.Automation.Commands.BcrCommand;

namespace Unit4.Automation
{
    internal class CostCentreHierarchy
    {
        private readonly ICache<SerializableCostCentreList> _costCentres;
        private readonly BcrOptions _options;

        public CostCentreHierarchy(ICache<SerializableCostCentreList> costCentres, BcrOptions options)
        {
            _costCentres = costCentres;
            _options = options;
        }

        public IEnumerable<IGrouping<string, CostCentre>> GetHierarchyByTier3()
        {
            var costCentres = _costCentres.Fetch().CostCentres;
            var filteredCostCentres = costCentres.Where(x => x.Matches(_options));
            return filteredCostCentres.GroupBy(x => x.Tier3, x => x);
        }
    }
}