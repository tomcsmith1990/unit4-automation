using System;
using ReportEngine.Provider.WebService;

namespace Unit4.ReportEngine
{
    internal class Unit4WebProvider : IDisposable
    {
        private readonly WebProvider _provider;

        public Unit4WebProvider(ProgramConfig config)
        {
            var connector = new Unit4WebConnector(new WindowsCredentialManager(), config).Create();
            _provider = new WebProvider(connector);
        }

        public void Dispose() => _provider.Dispose();

        public WebProvider Create() => _provider;
    }
}