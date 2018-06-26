using ReportEngine;
using ReportEngine.Interfaces;
using System.Data;
using Unit4.Automation.Interfaces;
using ReportEngine.Data.Sql;

namespace Unit4.Automation.ReportEngine
{
    internal class Unit4Engine : IUnit4Engine
    {
        public DataSet RunReport(string resql)
        {
            using (var webProvider = new Unit4WebProvider().Create())
            {
                var engine = new Engine(webProvider, ClientType.External);
                engine.InProcess = true;

                using (engine)
                {
                    var resqlProcessor = new ReSqlProcessor(engine);

                    return engine.RunReSql(resqlProcessor, resql);
                }
            }
        }
    }
}