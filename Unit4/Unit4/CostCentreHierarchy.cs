using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;

namespace Unit4
{
    internal class CostCentreHierarchy
    {
        private readonly CostCentreList _codeList = new CostCentreList();

        public IEnumerable<IGrouping<string, string>> GetHierarchyByTier3()
        {
            var costCentres = _codeList.GetCostCentres();
            return costCentres.GroupBy(x => x.Tier3, x => x.Tier4);
        }
    }
}