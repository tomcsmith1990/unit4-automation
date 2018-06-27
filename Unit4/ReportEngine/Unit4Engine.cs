using ReportEngine;
using ReportEngine.Interfaces;
using System.Data;
using Unit4.Automation.Interfaces;
using Unit4.Automation.Model;
using ReportEngine.Data.Sql;

namespace Unit4.Automation.ReportEngine
{
    internal class Unit4Engine : IUnit4Engine
    {
        private readonly ConfigOptions _config;

        public Unit4Engine(ConfigOptions config)
        {
            _config = config;
        }

        public DataSet RunReport(string resql)
        {
            using (var webProvider = new Unit4WebProvider(_config).Create())
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