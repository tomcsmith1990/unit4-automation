using ReportEngine.Base.Security;
using ReportEngine.Base.Data.Provider;
using Unit4.Automation.Interfaces;
using Unit4.Automation.Model;

namespace Unit4.Automation.ReportEngine
{
    internal class Unit4WebConnector
    {
        private readonly ICredentialManager _manager;
        private readonly ConfigOptions _config;

        public Unit4WebConnector(ICredentialManager manager, ConfigOptions config)
        {
            _manager = manager;
            _config = config;
        }
        
        public WebProviderConnector Create()
        {
            var credentials = _manager.CredentialsOrDefault;

            var agressoAuthenticator = new AgressoAuthenticator();
            agressoAuthenticator.Password = credentials.Password;
            
            var authenticators = new BaseAuthenticator[1]
            {
                agressoAuthenticator
            };
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