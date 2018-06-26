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
            var credentials = new FakeCredentials();
            var connector = new Unit4WebConnector(credentials, Mock.Of<ICredentialManager>()).Create();

            Assert.That(connector.Authenticator.Name, Is.EqualTo(credentials.Username));
        }

        [Test]
        public void TheConnector_ShouldHavePassword()
        {
            var credentials = new FakeCredentials();
            var connector = new Unit4WebConnector(credentials, Mock.Of<ICredentialManager>()).Create();
            var authenticator = connector.Authenticator as AgressoAuthenticator;

            Assert.That(SecureStringHelper.ToString(authenticator.Password), Is.EqualTo(SecureStringHelper.ToString(credentials.Password)));
        }

        [Test]
        public void TheConnector_ShouldHaveClient()
        {
            var credentials = new FakeCredentials();
            var connector = new Unit4WebConnector(credentials, Mock.Of<ICredentialManager>()).Create();
            var authenticator = connector.Authenticator as AgressoAuthenticator;

            Assert.That(authenticator.Client, Is.EqualTo(credentials.Client));
        }

        [Test]
        public void TheConnector_ShouldHaveSoapService()
        {
            var credentials = new FakeCredentials();
            var connector = new Unit4WebConnector(credentials, Mock.Of<ICredentialManager>()).Create();

            Assert.That(connector.Datasource, Is.EqualTo(credentials.SoapService));
        }

        [Test]
        public void TheConnector_ShouldAskCredentialsManagerForCredentials()
        {
            var manager = new Mock<ICredentialManager>();
            var credentials = new Mock<ICredentials>();
            credentials.Setup(x => x.Username).Returns("aDifferentUsername");
            manager.Setup(x => x.CredentialsOrDefault).Returns(credentials.Object);
            var connector = new Unit4WebConnector(new FakeCredentials(), manager.Object).Create();

            Assert.That(connector.Authenticator.Name, Is.EqualTo(credentials.Object.Username));
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