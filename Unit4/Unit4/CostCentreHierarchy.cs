using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;

namespace Unit4
{
    internal class CostCentreHierarchy
    {
        private readonly ICostCentres _costCentres = new CostCentreList();

        public CostCentreHierarchy(ICostCentres costCentres)
        {
            _costCentres = costCentres;
        }

        public IEnumerable<IGrouping<string, string>> GetHierarchyByTier3()
        {
            var costCentres = _costCentres.GetCostCentres();
            return costCentres.GroupBy(x => x.Tier3, x => x.Tier4);
        }
    }
}