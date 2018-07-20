using System.Data;

namespace Unit4.Automation.Interfaces
{
    public interface IUnit4Engine
    {
        DataSet RunReport(string resql);
    }
}