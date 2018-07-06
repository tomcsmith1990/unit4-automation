using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Unit4.Automation.Commands.BcrCommand;
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

        [Test, TestCaseSource(nameof(FilteredCostCentreOptions))]
        public void GivenCostCentreOption_ThenOnlyTheTier3ForThatCostCentreShouldBeIncluded(BcrOptions options)
        {
            var costCentres = new [] {
                new CostCentre() { Tier1 = "A1", Tier2 = "A2", Tier3 = "A3", Tier4 = "A4", Code = "A5" },
                new CostCentre() { Tier1 = "B1", Tier2 = "B2", Tier3 = "B3", Tier4 = "B4", Code = "B5" }
            };

            var hierarchy = new CostCentreHierarchy(new DummyCostCentres(costCentres), options);

            var filteredList = hierarchy.GetHierarchyByTier3();

            Assert.That(filteredList.Select(x => x.Key), Is.EquivalentTo(new [] { "A3" }));
        }

        private static IEnumerable<BcrOptions> FilteredCostCentreOptions
        {
            get
            {
                yield return new BcrOptions(Enumerable.Empty<string>(), Enumerable.Empty<string>(), Enumerable.Empty<string>(), Enumerable.Empty<string>(), new [] { "A5" }, null, false);
                yield return new BcrOptions(Enumerable.Empty<string>(), Enumerable.Empty<string>(), Enumerable.Empty<string>(), new [] { "A4" }, Enumerable.Empty<string>(), null, false);
                yield return new BcrOptions(Enumerable.Empty<string>(), Enumerable.Empty<string>(), new [] { "A3" }, Enumerable.Empty<string>(), Enumerable.Empty<string>(), null, false);
                yield return new BcrOptions(Enumerable.Empty<string>(), new [] { "A2" }, Enumerable.Empty<string>(), Enumerable.Empty<string>(), Enumerable.Empty<string>(), null, false);
                yield return new BcrOptions(new [] { "A1" }, Enumerable.Empty<string>(), Enumerable.Empty<string>(), Enumerable.Empty<string>(), Enumerable.Empty<string>(), null, false);
            }
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