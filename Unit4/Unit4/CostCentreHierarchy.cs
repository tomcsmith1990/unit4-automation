using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using Unit4.Interfaces;
using Unit4.Automation.Model;

namespace Unit4
{
    internal class CostCentreHierarchy
    {
        private readonly ICostCentres _costCentres = new CostCentreList();

        public CostCentreHierarchy(ICostCentres costCentres)
        {
            _costCentres = costCentres;
        }

        public IEnumerable<IGrouping<string, CostCentre>> GetHierarchyByTier3()
        {
            var costCentres = _costCentres.GetCostCentres();
            return costCentres.GroupBy(x => x.Tier3, x => x);
        }
    }
}