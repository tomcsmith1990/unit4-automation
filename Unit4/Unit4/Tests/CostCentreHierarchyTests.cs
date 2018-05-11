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
        public void GivenNoCostCentres_ThenThereShouldBeNoGroups()
        {
            var hierarchy = new CostCentreHierarchy(new DummyCostCentres());
            
            var tier3Hierarchy = hierarchy.GetHierarchyByTier3();

            Assert.That(tier3Hierarchy.Count(), Is.EqualTo(0));
        }

        [Test]
        public void GivenOneTier3_ThenThereShouldBeOneGroup()
        {
            var hierarchy = new CostCentreHierarchy(
                                new DummyCostCentres(
                                    new CostCentre() { Tier3 = "A", Tier4 = "B" },
                                    new CostCentre() { Tier3 = "A", Tier4 = "C" },
                                    new CostCentre() { Tier3 = "A", Tier4 = "D" }
                                ));
            
            var tier3Hierarchy = hierarchy.GetHierarchyByTier3();

            Assert.That(tier3Hierarchy.Count(), Is.EqualTo(1));
        }

        [Test]
        public void GivenTwoTier3s_ThenThereShouldBeTwoGroups()
        {
            var hierarchy = new CostCentreHierarchy(
                                new DummyCostCentres(
                                    new CostCentre() { Tier3 = "A", Tier4 = "B" },
                                    new CostCentre() { Tier3 = "C", Tier4 = "D" }
                                ));
            
            var tier3Hierarchy = hierarchy.GetHierarchyByTier3();

            Assert.That(tier3Hierarchy.Count(), Is.EqualTo(2));
        }

        private class DummyCostCentres : ICostCentres
        {
            private readonly CostCentre[] _costCentres;
            
            public DummyCostCentres(params CostCentre[] costCentres)
            {
                _costCentres = costCentres;
            }

            public IEnumerable<CostCentre> GetCostCentres()
            {
                return _costCentres; 
            }
        }
    }
}