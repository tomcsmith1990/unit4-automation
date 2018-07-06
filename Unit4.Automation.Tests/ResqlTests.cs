using NUnit.Framework;

namespace Unit4.Automation.Tests
{
    [TestFixture]
    internal class ResqlTests
    {
        [Test]
        public void GivenCostCentreParameter_TheReportShouldUseItAsCostCentre()
        {
            var report = Resql.Bcr(costCentre: "aCostCentre");

            Assert.That(report.Contains(".declare [Cost Centre] String 'aCostCentre'"), Is.True);
        }

        [Test]
        public void GivenTier3Parameter_TheReportShouldUseItAsTier3()
        {
            var report = Resql.Bcr(tier3: "aTier3");

            Assert.That(report.Contains(".declare [Service (Tier3)] String 'aTier3'"), Is.True);
        }

        [Test]
        public void GivenTier4Parameter_TheReportShouldUseItAsTier4()
        {
            var report = Resql.Bcr(tier4: "aTier4");

            Assert.That(report.Contains(".declare [Budget Group (Tier4)] String 'aTier4'"), Is.True);
        }
    }
}