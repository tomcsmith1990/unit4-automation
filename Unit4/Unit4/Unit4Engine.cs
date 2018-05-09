using System;
using ReportEngine;
using ReportEngine.Base.Data.Provider;
using ReportEngine.Interfaces;
using ReportEngine.Base.Security;
using ReportEngine.Base.Interfaces;
using ReportEngine.Provider.WebService;
using System.Data;
using ReportEngine.IO;
using ReportEngine.Data;

namespace Unit4
{
    internal class Unit4Engine
    {
        private readonly Credentials m_Credentials;

        public Unit4Engine(Credentials credentials)
        {
            m_Credentials = credentials;
        }

        public void RunReport(string inputFile, string outputFile)
        {
            var agressoAuthenticator = new AgressoAuthenticator();
            agressoAuthenticator.Password = SecureStringHelper.ToSecureString(m_Credentials.Password);
            
            var authenticators = new BaseAuthenticator[1]
            {
                agressoAuthenticator
            };
            var connector = new WebProviderConnector() 
            { 
                Name = "WebService",
                Authenticators = authenticators,
                Authenticator = agressoAuthenticator,
                Datasource = m_Credentials.SoapService
            };
            
            agressoAuthenticator.Name = m_Credentials.Username;
            agressoAuthenticator.Client = m_Credentials.Client;

            var engine = new Engine(new WebProvider(connector), ClientType.External);
            engine.InProcess = true;
            using (engine)
            {
                DataSet dataset = null;
                engine.RunReportEx(inputFile, outputFile, false);
            }
        }
    }
}