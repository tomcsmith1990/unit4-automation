using System;
using ReportEngine.Provider.WebService;
using Unit4.Automation.Model;

namespace Unit4.Automation.ReportEngine
{
    internal class Unit4WebProvider : IDisposable
    {
        private readonly WebProvider _provider;

        public Unit4WebProvider(ConfigOptions config)
        {
            var connector = new Unit4WebConnector(new WindowsCredentialManager(), config).Create();
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