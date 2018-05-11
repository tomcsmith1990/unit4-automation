using System;
using System.Collections.Generic;

namespace Unit4
{
    internal interface ICostCentres
    {
        IEnumerable<CostCentre> GetCostCentres();
    }
}