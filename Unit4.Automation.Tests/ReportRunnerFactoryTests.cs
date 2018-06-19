using Unit4.Automation.Model;
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

            Assert.That(factory.Create(options), Is.TypeOf(typeof(BcrReportRunner)));
        }

        [Test]
        public void GivenNullOptions_ThenWeShouldGetNullRunner()
        {
            var factory = new ReportRunnerFactory();

            var options = new NullOptions();

            Assert.That(factory.Create(options), Is.TypeOf(typeof(NullRunner)));
        }
    }
}