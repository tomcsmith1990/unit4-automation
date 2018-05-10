using System;
using ReportEngine;
using ReportEngine.Interfaces;
using ReportEngine.Base.Interfaces;
using System.Data;
using ReportEngine.IO;
using ReportEngine.Data;
using Unit4.Interfaces;

namespace Unit4
{
    internal class Unit4Engine
    {
        private readonly ICredentials m_Credentials;

        public Unit4Engine(ICredentials credentials)
        {
            m_Credentials = credentials;
        }

        public void RunReport(string inputFile, string outputFile)
        {
            var webProvider = new Unit4WebProvider(m_Credentials).Create();

            var engine = new Engine(webProvider, ClientType.External);
            engine.InProcess = true;
            using (engine)
            {
                engine.RunReportEx(inputFile, outputFile, false);
            }
        }
    }
}