using System;
using System.Collections.Generic;

namespace Unit4.Interfaces
{
    internal interface ICostCentres
    {
        IEnumerable<CostCentre> GetCostCentres();
    }
}