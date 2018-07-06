using System.Collections.Generic;
using System.Linq;

namespace Unit4.Automation.Model
{
    internal class SerializableCostCentreList
    {
        private IEnumerable<CostCentre> _costCentres;

        public IEnumerable<CostCentre> CostCentres
        {
            get { return _costCentres == null ? Enumerable.Empty<CostCentre>() : _costCentres; }
            set { _costCentres = value; }
        }
    }
}