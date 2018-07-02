using NUnit.Framework;
using System.Linq;
using Unit4.Automation.Interfaces;
using Unit4.Automation.Model;

namespace Unit4.Automation.Tests
{
    [TestFixture]
    internal class CostCentreHierarchyTests
    {
        [Test]
        public void GivenNoCostCentres_ThenThereShouldBeNoGroups()
        {
            var hierarchy = new CostCentreHierarchy(new DummyCostCentres(), new BcrOptions());
            
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
                                ), new BcrOptions());
            
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
                                ), new BcrOptions());
            
            var tier3Hierarchy = hierarchy.GetHierarchyByTier3();

            Assert.That(tier3Hierarchy.Count(), Is.EqualTo(2));
        }

        [Test]
        public void GivenSomeCostCentresInTheTier_ThenThereShouldBeAllCostCentresInThatGroup()
        {
            var costCentres = new [] {
                new CostCentre() { Tier3 = "A", Tier4 = "B" },
                new CostCentre() { Tier3 = "A", Tier4 = "C" },
                new CostCentre() { Tier3 = "A", Tier4 = "D" }
            };
            var hierarchy = new CostCentreHierarchy(new DummyCostCentres(costCentres), new BcrOptions());
            
            var actualCostCentres = hierarchy.GetHierarchyByTier3().Single().ToList();

            Assert.That(actualCostCentres, Is.EquivalentTo(costCentres));
        }

        private class DummyCostCentres : ICache<SerializableCostCentreList>
        {
            private readonly CostCentre[] _costCentres;
            
            public DummyCostCentres(params CostCentre[] costCentres)
            {
                _costCentres = costCentres;
            }

            public SerializableCostCentreList Fetch()
            {
                return new SerializableCostCentreList() { CostCentres = _costCentres };
            }
        }
    }
}