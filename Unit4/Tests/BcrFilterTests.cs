using System;
using Unit4.Automation;
using NUnit.Framework;
using Unit4.Automation.Model;
using Unit4.Automation.Interfaces;
using System.Linq;

namespace Unit4.Automation.Tests
{
    [TestFixture]
    public class BcrFilterTests
    {
        [Test]
        public void GivenTier2Option_ThenLinesNotMatchingThatTier2ShouldNotBeIncluded()
        {
            var options = new BcrOptions("tier2");

            var filter = new BcrFilter(options);

            var bcr = new Bcr(new BcrLine[] {
                    new BcrLine() {
                        CostCentre = new CostCentre() {
                            Tier2 = "notTheRightTier2"
                        }
                    },
                });

            Assert.That(filter.Use(bcr).Lines, Is.Empty);
        }

        [Test]
        public void GivenTier2Option_ThenLinesMatchingThatTier2ShouldBeIncluded()
        {
            var options = new BcrOptions("tier2");

            var filter = new BcrFilter(options);

            var bcr = new Bcr(new BcrLine[] {
                    new BcrLine() {
                        CostCentre = new CostCentre() {
                            Tier2 = "tier2"
                        }
                    },
                });

            Assert.That(filter.Use(bcr).Lines.ToList(), Has.Count.EqualTo(1));
        }

        [Test]
        public void GivenNoTier2Option_ThenAllLinesShouldBeIncluded()
        {
            var options = new BcrOptions();

            var filter = new BcrFilter(options);

            var bcr = new Bcr(new BcrLine[] {
                    new BcrLine() {
                        CostCentre = new CostCentre() {
                            Tier2 = "tier2"
                        }
                    },
                });

            Assert.That(filter.Use(bcr).Lines.ToList(), Has.Count.EqualTo(1));
        }
    }
}