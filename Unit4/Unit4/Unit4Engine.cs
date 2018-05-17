using System;
using ReportEngine;
using ReportEngine.Interfaces;
using ReportEngine.Base.Interfaces;
using System.Data;
using ReportEngine.IO;
using ReportEngine.Data;
using Unit4.Automation.Interfaces;
using ReportEngine.Data.Sql;
using System.IO;

namespace Unit4
{
    internal class Unit4Engine : IUnit4Engine
    {
        private readonly ICredentials m_Credentials;

        public Unit4Engine(ICredentials credentials)
        {
            m_Credentials = credentials;
        }

        public DataSet RunReport(string resql)
        {
            using (var webProvider = new Unit4WebProvider(m_Credentials).Create())
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