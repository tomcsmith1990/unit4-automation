using System.Data;
using ReportEngine;
using ReportEngine.Data.Sql;
using ReportEngine.Interfaces;
using Unit4.Automation.Interfaces;

namespace Unit4.Automation.ReportEngine
{
    internal class Unit4Engine : IUnit4Engine
    {
        private readonly ProgramConfig _config;

        public Unit4Engine(ProgramConfig config)
        {
            _config = config;
        }

        public DataSet RunReport(string resql)
        {
            using (var webProvider = new Unit4WebProvider(_config).Create())
            {
                var engine = new Engine(webProvider, ClientType.External) {InProcess = true};

                using (engine)
                {
                    var resqlProcessor = new ReSqlProcessor(engine);

                    return engine.RunReSql(resqlProcessor, resql);
                }
            }
        }
    }
}