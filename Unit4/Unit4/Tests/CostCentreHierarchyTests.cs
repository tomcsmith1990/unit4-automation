using System;
using Unit4;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Unit4.Tests
{
    [TestFixture]
    public class CostCentreHierarchyTests
    {
        [Test]
        public void GivenOneTier3_ThenThereShouldBeOneGroup()
        {
            var hierarchy = new CostCentreHierarchy(new DummyCostCentres());
            
            var tier3Hierarchy = hierarchy.GetHierarchyByTier3();

            Assert.That(tier3Hierarchy.Count(), Is.EqualTo(1));
        }

        private class DummyCostCentres : ICostCentres
        {
            public IEnumerable<CostCentre> GetCostCentres()
            {
                yield return new CostCentre() { Tier3 = "A", Tier4 = "B" };
                yield return new CostCentre() { Tier3 = "A", Tier4 = "C" };
                yield return new CostCentre() { Tier3 = "A", Tier4 = "D" };
            }
        }
    }
}