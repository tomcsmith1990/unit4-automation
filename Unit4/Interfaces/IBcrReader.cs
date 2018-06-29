using Unit4.Automation.Model;
using System.Linq;

namespace Unit4.Automation.Interfaces
{
    internal interface IBcrReader
    {
        Bcr Read();
        System.Collections.Generic.IEnumerable<CostCentre> GetCostCentres();
        Bcr Read(System.Collections.Generic.IEnumerable<IGrouping<string, CostCentre>> hierarchy);
    }
}