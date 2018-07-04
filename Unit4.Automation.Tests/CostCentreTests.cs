using NUnit.Framework;
using Unit4.Automation.Model;

namespace Unit4.Automation.Tests
{
    [TestFixture]
    internal class CostCentreTests
    {
        [Test]
        public void CostCentreComparisonShouldUseTier1Comparison()
        {
            var costCentre1 = new CostCentre() { Tier1 = "1" };
            var costCentre2 = new CostCentre() { Tier1 = "2" };

            Assert.That(costCentre1, Is.LessThan(costCentre2));
        }
    }
}