using NUnit.Framework;
using Unit4.Automation.Model;
using System.Collections.Generic;

namespace Unit4.Automation.Tests
{
    [TestFixture]
    internal class BcrLineTests
    {
        [Test]
        public void GivenNullValue_ThenTheObjectShouldBeGreater()
        {
            var line = new BcrLine();
            var result = line.CompareTo(null) > 0;

            Assert.That(result, Is.True);
        }

        [Test, TestCaseSource(nameof(Comparisons))]
        public void TestBcrLineCompareTo(BcrLine first, BcrLine second)
        {
            Assert.That(first, Is.LessThan(second));
        }

        private static IEnumerable<TestCaseData> Comparisons
        {
            get
            {
                yield return new TestCaseData(new BcrLine() { CostCentre = new CostCentre() { Tier1 = "1" } }, new BcrLine() { CostCentre = new CostCentre() { Tier1 = "2" } }).SetName("GivenLesserCostCentre_ThenTheBcrLineShouldBeLesser");
                yield return new TestCaseData(new BcrLine() { CostCentre = new CostCentre() { Tier1 = "1" }, Account = "1" }, new BcrLine() { CostCentre = new CostCentre() { Tier1 = "1" }, Account = "2" }).SetName("GivenEqualCostCentre_ThenTheLesserAccountShouldBeUsed");
            }
        }
    }
}