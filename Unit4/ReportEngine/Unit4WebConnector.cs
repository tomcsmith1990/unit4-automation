using ReportEngine.Base.Security;
using ReportEngine.Base.Data.Provider;
using Unit4.Automation.Interfaces;

namespace Unit4.Automation.ReportEngine
{
    internal class Unit4WebConnector
    {
        private readonly ICredentialManager _manager;

        public Unit4WebConnector(ICredentialManager manager)
        {
            _manager = manager;
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
                Datasource = credentials.SoapService
            };
            
            agressoAuthenticator.Name = credentials.Username;
            agressoAuthenticator.Client = credentials.Client;

            return connector;
        }
    }
}