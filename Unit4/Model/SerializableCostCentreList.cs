using System;
using System.Collections.Generic;

namespace Unit4.Automation.Model
{
    internal class SerializableCostCentreList
    {
        public CostCentre[] CostCentres { get; set; }

        public IEnumerable<CostCentre> AsEnumerable()
        {
            return CostCentres;
        }
    }
}