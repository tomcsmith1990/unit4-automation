using System;
using ReportEngine.Provider.WebService;
using Unit4.Automation.Interfaces;

namespace Unit4.Automation.ReportEngine
{
    internal class Unit4WebProvider : IDisposable
    {
        private readonly ICredentials m_Credentials;
        private readonly WebProvider _provider;

        public Unit4WebProvider(ICredentials credentials)
        {
            m_Credentials = credentials;

            var connector = new Unit4WebConnector(credentials).Create();
            _provider = new WebProvider(connector);
        }

        public WebProvider Create()
        {
            return _provider;
        }

        public void Dispose()
        {
            _provider.Dispose();
        }
    }
}