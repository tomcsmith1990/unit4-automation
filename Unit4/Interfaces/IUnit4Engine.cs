using System.Data;

namespace Unit4.Automation.Interfaces
{
    internal interface IUnit4Engine
    {
        DataSet RunReport(string resql);
    }
}