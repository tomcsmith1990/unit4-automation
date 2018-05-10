using System;
using ReportEngine;
using ReportEngine.Interfaces;
using ReportEngine.Base.Interfaces;
using System.Data;
using ReportEngine.IO;
using ReportEngine.Data;
using Unit4.Interfaces;
using ReportEngine.Data.Sql;
using System.IO;

namespace Unit4
{
    internal class Unit4Engine
    {
        const string _resql = @".name [GL-BAL-001 : General Balances Monitoring Report]

.declare [Directorate (Tier1)] String ''

.declare [Service Group (Tier2)] String ''

.declare [Budget Group (Tier4)] String ''

.declare [Cost Centre] String '{0}'

.declare [Service (Tier3)] String ''

.declare [Account] String ''

.query [GL-BAL-001 : General Balances Monitoring Report] 
    agr_getBrowser 'GL-BAL-001 : General Balances Monitoring Report', r0r0r0r3dim2_eq='$?[Directorate (Tier1)]', r0r0r3dim2_eq='$?[Service Group (Tier2)]', r3dim2_eq='$?[Budget Group (Tier4)]', dim2_eq='$?[Cost Centre]', r0r3dim2_eq='$?[Service (Tier3)]', dim1_eq='$?[Account]'
.endQuery";
        private readonly ICredentials m_Credentials;

        public Unit4Engine(ICredentials credentials)
        {
            m_Credentials = credentials;
        }

        public DataSet RunReport(string costCentre)
        {
            var webProvider = new Unit4WebProvider(m_Credentials).Create();

            var engine = new Engine(webProvider, ClientType.External);
            engine.InProcess = true;

            using (engine)
            {
                var resqlProcessor = new ReSqlProcessor(engine);

                return engine.RunReSql(resqlProcessor, string.Format(_resql, costCentre));
            }
        }
    }
}