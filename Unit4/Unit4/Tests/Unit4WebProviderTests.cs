using System;
using NUnit.Framework;
using Unit4.Automation.Interfaces;
using ReportEngine.Base.Data.Provider;
using ReportEngine.Base.Security;
using Unit4.Automation.ReportEngine;

namespace Unit4.Automation.Tests
{
    [TestFixture]
    public class Unit4WebProviderTests
    {
        [Test]
        public void TheConnector_ShouldHaveUsername()
        {
            var credentials = new FakeCredentials();
            var connector = new Unit4WebConnector(credentials).Create();

            Assert.That(connector.Authenticator.Name, Is.EqualTo(credentials.Username));
        }

        [Test]
        public void TheConnector_ShouldHavePassword()
        {
            var credentials = new FakeCredentials();
            var connector = new Unit4WebConnector(credentials).Create();
            var authenticator = connector.Authenticator as AgressoAuthenticator;

            Assert.That(SecureStringHelper.ToString(authenticator.Password), Is.EqualTo(credentials.Password));
        }

        [Test]
        public void TheConnector_ShouldHaveClient()
        {
            var credentials = new FakeCredentials();
            var connector = new Unit4WebConnector(credentials).Create();
            var authenticator = connector.Authenticator as AgressoAuthenticator;

            Assert.That(authenticator.Client, Is.EqualTo(credentials.Client));
        }

        [Test]
        public void TheConnector_ShouldHaveSoapService()
        {
            var credentials = new FakeCredentials();
            var connector = new Unit4WebConnector(credentials).Create();

            Assert.That(connector.Datasource, Is.EqualTo(credentials.SoapService));
        }

        private class FakeCredentials : ICredentials
        {
            public string Username { get { return "username"; } }
            public string Password { get { return "password"; } }
            public string Client { get { return "client"; } }
            public string SoapService { get { return "soapService"; } }
        }
    }
}