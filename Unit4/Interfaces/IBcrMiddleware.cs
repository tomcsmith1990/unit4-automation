using Unit4.Automation.Model;
using System.Collections.Generic;

namespace Unit4.Automation.Interfaces
{
    internal interface IBcrMiddleware
    {
        IEnumerable<CostCentre> Use(IEnumerable<CostCentre> bcr);
        Bcr Use(Bcr bcr);
    }
}