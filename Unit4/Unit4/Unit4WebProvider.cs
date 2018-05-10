using System;
using ReportEngine.Provider.WebService;
using Unit4.Interfaces;

namespace Unit4
{
    internal class Unit4WebProvider
    {
        private readonly ICredentials m_Credentials;

        public Unit4WebProvider(ICredentials credentials)
        {
            m_Credentials = credentials;
        }

        public WebProvider Create()
        {
            var connector = new Unit4WebConnector(m_Credentials).Create();

            return new WebProvider(connector);
        }
    }
}