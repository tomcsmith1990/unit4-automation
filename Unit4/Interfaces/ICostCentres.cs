using System;
using System.Collections.Generic;
using Unit4.Automation.Model;

namespace Unit4.Automation.Interfaces
{
    internal interface ICostCentres
    {
        IEnumerable<CostCentre> GetCostCentres();
    }
}