using System.Security;
using NUnit.Framework;
using ReportEngine.Base.Data.Provider;
using ReportEngine.Base.Security;
using Unit4.Automation.Interfaces;
using Unit4.Automation.ReportEngine;

namespace Unit4.Automation.Tests
{
    [TestFixture]
    internal class Unit4WebConnectorTests
    {
        [Test]
        public void TheConnector_ShouldHaveUsername()
        {
            var manager = new FakeCredentialManager();
            var config = new ProgramConfig(() => 0, () => "foo");
            var connector = new Unit4WebConnector(manager, config).Create();

            Assert.That(connector.Authenticator.Name, Is.EqualTo(manager.Credentials.Username));
        }

        [Test]
        public void TheConnector_ShouldHavePassword()
        {
            var manager = new FakeCredentialManager();
            var config = new ProgramConfig(() => 0, () => "foo");
            var connector = new Unit4WebConnector(manager, config).Create();
            var authenticator = connector.Authenticator as AgressoAuthenticator;

            Assert.That(
                SecureStringHelper.ToString(authenticator.Password),
                Is.EqualTo(SecureStringHelper.ToString(manager.Credentials.Password)));
        }

        [Test]
        public void TheConnector_ShouldHaveClient()
        {
            var manager = new FakeCredentialManager();
            var config = new ProgramConfig(() => 1234, () => "foo");
            var connector = new Unit4WebConnector(manager, config).Create();
            var authenticator = connector.Authenticator as AgressoAuthenticator;

            Assert.That(authenticator.Client, Is.EqualTo(config.Client.ToString()));
        }

        [Test]
        public void TheConnector_ShouldHaveSoapService()
        {
            var manager = new FakeCredentialManager();
            var config = new ProgramConfig(() => 0, () => "foo");
            var connector = new Unit4WebConnector(manager, config).Create();

            Assert.That(connector.Datasource, Is.EqualTo(config.Url));
        }

        private class FakeCredentialManager : ICredentialManager
        {
            public ICredentials Credentials => new FakeCredentials();
        }

        private class FakeCredentials : ICredentials
        {
            public string Username => "username";
            public SecureString Password => SecureStringHelper.ToSecureString("password");
        }
    }
}