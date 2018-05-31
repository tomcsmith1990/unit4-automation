using System;
using Unit4.Automation;
using NUnit.Framework;
using Unit4.Automation.Model;
using Unit4.Automation.Interfaces;

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
    }

    internal class BcrFilter : IBcrMiddleware
    {
        public BcrFilter(BcrOptions options)
        {

        }

        public Bcr Use(Bcr bcr)
        {
            throw new NotImplementedException();
        }
    }
}