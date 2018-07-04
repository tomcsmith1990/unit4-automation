using NUnit.Framework;
using Unit4.Automation.Model;

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

        [Test]
        public void GivenLesserCostCentre_ThenTheBcrLineShouldBeLesser()
        {
            var line1 = new BcrLine() { CostCentre = new CostCentre() { Tier1 = "1" } };
            var line2 = new BcrLine() { CostCentre = new CostCentre() { Tier1 = "2" } };

            Assert.That(line1, Is.LessThan(line2));
        }

        [Test]
        public void GivenEqualCostCentre_ThenTheLesserAccountShouldBeUsed()
        {
            var line1 = new BcrLine() { CostCentre = new CostCentre() { Tier1 = "1" }, Account = "1" };
            var line2 = new BcrLine() { CostCentre = new CostCentre() { Tier1 = "1" }, Account = "2" };

            Assert.That(line1, Is.LessThan(line2));
        }
    }
}