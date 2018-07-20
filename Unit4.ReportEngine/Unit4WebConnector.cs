using System;
using ReportEngine.Base.Data.Provider;
using Unit4.Automation.Interfaces;

namespace Unit4.Automation.ReportEngine
{
    internal class Unit4WebConnector
    {
        private readonly ProgramConfig _config;
        private readonly ICredentialManager _manager;

        public Unit4WebConnector(ICredentialManager manager, ProgramConfig config)
        {
            _manager = manager;
            _config = config;
        }

        public WebProviderConnector Create()
        {
            var credentials = _manager.Credentials;

            var agressoAuthenticator = new AgressoAuthenticator() { Password = credentials.Password };
            var authenticators = new BaseAuthenticator[] { agressoAuthenticator };

            if (_config.Url == null)
            {
                throw new ApplicationException("The Unit4 SOAP service URL is not set in the config file.");
            }

            var connector = new WebProviderConnector()
            {
                Name = "WebService",
                Authenticators = authenticators,
                Authenticator = agressoAuthenticator,
                Datasource = _config.Url
            };

            agressoAuthenticator.Name = credentials.Username;
            agressoAuthenticator.Client = _config.Client.ToString();

            return connector;
        }
    }
}