using System;
using ReportEngine.Base.Security;
using ReportEngine.Base.Data.Provider;
using Unit4.Automation.Interfaces;

namespace Unit4.Automation.ReportEngine
{
    internal class Unit4WebConnector
    {
        private readonly ICredentials m_Credentials;

        public Unit4WebConnector(ICredentials credentials)
        {
            m_Credentials = credentials;
        }
        public WebProviderConnector Create()
        {
            var agressoAuthenticator = new AgressoAuthenticator();
            agressoAuthenticator.Password = SecureStringHelper.ToSecureString(m_Credentials.Password);
            
            var authenticators = new BaseAuthenticator[1]
            {
                agressoAuthenticator
            };
            var connector = new WebProviderConnector() 
            { 
                Name = "WebService",
                Authenticators = authenticators,
            };

            connector.Read();
            connector.Authenticator = agressoAuthenticator;

            SetCredentialsIfNotFoundFromRegistry(connector, agressoAuthenticator);

            return connector;
        }

        private void SetCredentialsIfNotFoundFromRegistry(WebProviderConnector connector, AgressoAuthenticator agressoAuthenticator)
        {
            if (string.IsNullOrEmpty(connector.Datasource))
            {
                connector.Datasource = m_Credentials.SoapService;
            }

            if (string.IsNullOrEmpty(agressoAuthenticator.Name))
            {
                agressoAuthenticator.Name = m_Credentials.Username;
                agressoAuthenticator.Client = m_Credentials.Client;
            }
        }
    }
}