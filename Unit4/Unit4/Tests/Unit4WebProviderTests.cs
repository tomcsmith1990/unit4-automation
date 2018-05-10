using System;
using NUnit.Framework;
using Unit4.Interfaces;

namespace Unit4.Tests
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
        public void ShouldFail()
        {
            Assert.That(true, Is.False);
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