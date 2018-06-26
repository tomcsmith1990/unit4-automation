using NUnit.Framework;
using Unit4.Automation.Interfaces;
using ReportEngine.Base.Data.Provider;
using ReportEngine.Base.Security;
using Unit4.Automation.ReportEngine;
using Moq;
using System.Security;

namespace Unit4.Automation.Tests
{
    [TestFixture]
    internal class Unit4WebConnectorTests
    {
        [Test]
        public void TheConnector_ShouldHaveUsername()
        {
            var manager = new FakeCredentialManager();
            var connector = new Unit4WebConnector(manager).Create();

            Assert.That(connector.Authenticator.Name, Is.EqualTo(manager.CredentialsOrDefault.Username));
        }

        [Test]
        public void TheConnector_ShouldHavePassword()
        {
            var manager = new FakeCredentialManager();
            var connector = new Unit4WebConnector(manager).Create();
            var authenticator = connector.Authenticator as AgressoAuthenticator;

            Assert.That(SecureStringHelper.ToString(authenticator.Password), Is.EqualTo(SecureStringHelper.ToString(manager.CredentialsOrDefault.Password)));
        }

        [Test]
        public void TheConnector_ShouldHaveClient()
        {
            var manager = new FakeCredentialManager();
            var connector = new Unit4WebConnector(manager).Create();
            var authenticator = connector.Authenticator as AgressoAuthenticator;

            Assert.That(authenticator.Client, Is.EqualTo(manager.CredentialsOrDefault.Client));
        }

        [Test]
        public void TheConnector_ShouldHaveSoapService()
        {
            var manager = new FakeCredentialManager();
            var connector = new Unit4WebConnector(manager).Create();

            Assert.That(connector.Datasource, Is.EqualTo(manager.CredentialsOrDefault.SoapService));
        }

        private class FakeCredentialManager : ICredentialManager
        {
            public ICredentials CredentialsOrDefault { get { return new FakeCredentials(); } }
        }

        private class FakeCredentials : ICredentials
        {
            public string Username { get { return "username"; } }
            public SecureString Password { get { return SecureStringHelper.ToSecureString("password"); } }
            public string Client { get { return "client"; } }
            public string SoapService { get { return "soapService"; } }
        }
    }
}