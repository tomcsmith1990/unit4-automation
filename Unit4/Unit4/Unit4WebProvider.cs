using System;
using ReportEngine.Provider.WebService;
using Unit4.Interfaces;

namespace Unit4
{
    internal class Unit4WebProvider : IDisposable
    {
        private readonly ICredentials m_Credentials;
        private readonly WebProvider m_Provider;

        public Unit4WebProvider(ICredentials credentials)
        {
            m_Credentials = credentials;

            var connector = new Unit4WebConnector(credentials).Create();
            m_Provider = new WebProvider(connector);
        }

        public WebProvider Create()
        {
            return m_Provider;
        }

        public void Dispose()
        {
            m_Provider.Dispose();
        }
    }
}