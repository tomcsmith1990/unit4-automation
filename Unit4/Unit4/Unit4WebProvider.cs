using System;
using ReportEngine.Provider.WebService;
using ReportEngine.Base.Security;
using ReportEngine.Base.Data.Provider;

namespace Unit4
{
    internal class Unit4WebProvider
    {
        private readonly Credentials m_Credentials;

        public Unit4WebProvider(Credentials credentials)
        {
            m_Credentials = credentials;
        }

        public WebProvider Create()
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

            return new WebProvider(connector);
        }
    }
}