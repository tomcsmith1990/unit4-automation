using System;
using System.Collections.Generic;
using Unit4.Automation.Model;

namespace Unit4.Interfaces
{
    internal interface ICostCentres
    {
        IEnumerable<CostCentre> GetCostCentres();
    }
}