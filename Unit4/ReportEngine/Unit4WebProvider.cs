using System;
using ReportEngine.Provider.WebService;
using Unit4.Automation.Interfaces;
using Unit4.Automation;

namespace Unit4.Automation.ReportEngine
{
    internal class Unit4WebProvider : IDisposable
    {
        private readonly WebProvider _provider;

        public Unit4WebProvider()
        {
            var connector = new Unit4WebConnector(new WindowsCredentialManager()).Create();
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