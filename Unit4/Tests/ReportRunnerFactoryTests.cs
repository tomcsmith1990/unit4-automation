using System;
using Unit4.Automation.Model;
using Unit4.Automation.Interfaces;
using NUnit.Framework;

namespace Unit4.Automation.Tests
{
    [TestFixture]
    internal class ReportRunnerFactoryTests
    {
        [Test]
        public void GivenBcrOptions_ThenWeShouldGetBcrRunner()
        {
            var factory = new ReportRunnerFactory();

            var options = new BcrOptions();

            Assert.That(factory.Create(options), Is.TypeOf(typeof(ReportRunner)));
        }
    }

    internal class ReportRunnerFactory
    {
        public ReportRunner Create(IOptions options)
        {
            return new ReportRunner();
        }
    }
}