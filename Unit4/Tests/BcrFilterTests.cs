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

            var bcr = new Bcr();
            bcr.Lines = new BcrLine[] {
                    new BcrLine() {
                        CostCentre = new CostCentre() {
                            Tier2 = "notTheRightTier2"
                        }
                    },
                };

            Assert.That(filter.Use(bcr).Lines, Is.Empty);
        }

        [Test]
        public void GivenTier2Option_ThenLinesMatchingThatTier2ShouldBeIncluded()
        {
            var options = new BcrOptions("tier2");

            var filter = new BcrFilter(options);

            var bcr = new Bcr();
            bcr.Lines = new BcrLine[] {
                    new BcrLine() {
                        CostCentre = new CostCentre() {
                            Tier2 = "tier2"
                        }
                    },
                };

            Assert.That(filter.Use(bcr).Lines, Has.Count.EqualTo(1));
        }

        [Test]
        public void GivenNoTier2Option_ThenAllLinesShouldBeIncluded()
        {
            var options = new BcrOptions();

            var filter = new BcrFilter(options);

            var bcr = new Bcr();
            bcr.Lines = new BcrLine[] {
                    new BcrLine() {
                        CostCentre = new CostCentre() {
                            Tier2 = "tier2"
                        }
                    },
                };

            Assert.That(filter.Use(bcr).Lines, Has.Count.EqualTo(1));
        }
    }

    internal class BcrFilter : IBcrMiddleware
    {
        private readonly BcrOptions _options;

        public BcrFilter(BcrOptions options)
        {
            _options = options;
        }

        public Bcr Use(Bcr bcr)
        {
            var newBcr = new Bcr();
            newBcr.Lines = _options.Tier2 == null ? bcr.Lines.ToList() : bcr.Lines.Where(x => x.CostCentre.Tier2.Equals(_options.Tier2)).ToList();
            return newBcr;
        }
    }
}