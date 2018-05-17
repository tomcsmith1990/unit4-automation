using System;
using ReportEngine.Base.Security;
using ReportEngine.Base.Data.Provider;
using Unit4.Automation.Interfaces;

namespace Unit4
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
                Authenticator = agressoAuthenticator,
                Datasource = m_Credentials.SoapService
            };
            
            agressoAuthenticator.Name = m_Credentials.Username;
            agressoAuthenticator.Client = m_Credentials.Client;

            return connector;
        }
    }
}