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

        [Test]
        public void GivenEqualTier1_ThenCostCentreComparisonShouldUseTier2Comparison()
        {
            var costCentre1 = new CostCentre() { Tier1 = "1", Tier2 = "1" };
            var costCentre2 = new CostCentre() { Tier1 = "1", Tier2 = "2" };

            Assert.That(costCentre1, Is.LessThan(costCentre2));
        }

        [Test]
        public void GivenEqualTier2_ThenCostCentreComparisonShouldUseTier3Comparison()
        {
            var costCentre1 = new CostCentre() { Tier1 = "1", Tier2 = "1", Tier3 = "1" };
            var costCentre2 = new CostCentre() { Tier1 = "1", Tier2 = "1", Tier3 = "2" };

            Assert.That(costCentre1, Is.LessThan(costCentre2));
        }

        [Test]
        public void GivenEqualTier3_ThenCostCentreComparisonShouldUseTier4Comparison()
        {
            var costCentre1 = new CostCentre() { Tier1 = "1", Tier2 = "1", Tier3 = "1", Tier4 = "1" };
            var costCentre2 = new CostCentre() { Tier1 = "1", Tier2 = "1", Tier3 = "1", Tier4 = "2" };

            Assert.That(costCentre1, Is.LessThan(costCentre2));
        }
    }
}