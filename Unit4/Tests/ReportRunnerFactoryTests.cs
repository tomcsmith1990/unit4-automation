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

        [Test]
        public void GivenNullOptions_ThenWeShouldGetNullRunner()
        {
            var factory = new ReportRunnerFactory();

            var options = new NullOptions();

            Assert.That(factory.Create(options), Is.TypeOf(typeof(NullRunner)));
        }
    }

    internal class ReportRunnerFactory
    {
        public IRunner Create(IOptions options)
        {
            var bcrOptions = options as BcrOptions;
            if (bcrOptions != null)
            {
                return new ReportRunner();
            }

            return new NullRunner();
        }
    }
}