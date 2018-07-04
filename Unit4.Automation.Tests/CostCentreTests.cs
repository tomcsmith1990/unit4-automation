using NUnit.Framework;
using Unit4.Automation.Model;
using System.Collections.Generic;

namespace Unit4.Automation.Tests
{
    [TestFixture]
    internal class CostCentreTests
    {
        [Test]
        public void GivenNullValue_ThenTheCostCentreShouldBeGreater()
        {
            var costCentre1 = new CostCentre() { Tier1 = "1" };
            var result = costCentre1.CompareTo(null) > 0;

            Assert.That(result, Is.True);
        }

        [Test]
        public void GivenCostCentresWithNullValues_ThenTheyAreEqual()
        {
            var costCentre1 = new CostCentre();
            Assert.That(new CostCentre(), Is.Not.LessThan(costCentre1).And.Not.GreaterThan(costCentre1));
        }

        [Test, TestCaseSource(nameof(Comparisons))]
        public void CostCentreComparisonShouldUseTier1Comparison(CostCentre first, CostCentre second)
        {
            Assert.That(first, Is.LessThan(second));
        }

        private static IEnumerable<TestCaseData> Comparisons
        {
            get
            {
                yield return new TestCaseData(new CostCentre() { Tier1 = "1" }, new CostCentre() { Tier1 = "2" }).SetName("GivenLesserTier1_ThenTheCostCentreShouldBeLesser");
                yield return new TestCaseData(new CostCentre() { Tier1 = "1", Tier2 = "1" }, new CostCentre() { Tier1 = "1", Tier2 = "2" }).SetName("GivenEqualTier1_ThenLesserTier2ShouldBeUsed");
                yield return new TestCaseData(new CostCentre() { Tier1 = "1", Tier2 = "1", Tier3 = "1" }, new CostCentre() { Tier1 = "1", Tier2 = "1", Tier3 = "2" }).SetName("GivenEqualTier2_ThenLesserTier3ShouldBeUsed");
                yield return new TestCaseData(new CostCentre() { Tier1 = "1", Tier2 = "1", Tier3 = "1", Tier4 = "1" }, new CostCentre() { Tier1 = "1", Tier2 = "1", Tier3 = "1", Tier4 = "2" }).SetName("GivenEqualTier3_ThenLesserTier4ShouldBeUsed");
                yield return new TestCaseData(new CostCentre() { Tier1 = "1", Tier2 = "1", Tier3 = "1", Tier4 = "1", Code = "1" }, new CostCentre() { Tier1 = "1", Tier2 = "1", Tier3 = "1", Tier4 = "1", Code = "2" }).SetName("GivenEqualTier4_ThenLesserCostCentreShouldBeUsed");
            }
        }
    }
}