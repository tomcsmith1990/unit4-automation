using Unit4.Automation.Model;

namespace Unit4.Automation.Interfaces
{
    internal interface ICostCentresProvider
    {
        SerializableCostCentreList GetCostCentres();
    }
}