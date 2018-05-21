using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using Unit4.Automation.Interfaces;
using Unit4.Automation.Model;

namespace Unit4.Automation
{
    internal class CostCentreHierarchy
    {
        private readonly ICache<SerializableCostCentreList> _costCentres;

        public CostCentreHierarchy(ICache<SerializableCostCentreList> costCentres)
        {
            _costCentres = costCentres;
        }

        public IEnumerable<IGrouping<string, CostCentre>> GetHierarchyByTier3()
        {
            var costCentres = _costCentres.Fetch().AsEnumerable();
            return costCentres.GroupBy(x => x.Tier3, x => x);
        }
    }
}