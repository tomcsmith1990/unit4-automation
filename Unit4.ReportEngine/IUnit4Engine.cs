using System.Data;

namespace Unit4.ReportEngine
{
    public interface IUnit4Engine
    {
        DataSet RunReport(string resql);
    }
}