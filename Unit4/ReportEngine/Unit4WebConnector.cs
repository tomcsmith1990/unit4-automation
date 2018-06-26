using ReportEngine.Base.Security;
using ReportEngine.Base.Data.Provider;
using Unit4.Automation.Interfaces;

namespace Unit4.Automation.ReportEngine
{
    internal class Unit4WebConnector
    {
        private readonly ICredentials _credentials;
        private readonly ICredentialManager _manager;

        public Unit4WebConnector(ICredentials credentials, ICredentialManager manager)
        {
            _credentials = credentials;
            _manager = manager;
        }
        
        public WebProviderConnector Create()
        {
            var c = _manager.CredentialsOrDefault;

            var agressoAuthenticator = new AgressoAuthenticator();
            agressoAuthenticator.Password = _credentials.Password;
            
            var authenticators = new BaseAuthenticator[1]
            {
                agressoAuthenticator
            };
            var connector = new WebProviderConnector() 
            { 
                Name = "WebService",
                Authenticators = authenticators,
                Authenticator = agressoAuthenticator,
                Datasource = _credentials.SoapService
            };
            
            agressoAuthenticator.Name = _credentials.Username;
            agressoAuthenticator.Client = _credentials.Client;

            return connector;
        }
    }
}