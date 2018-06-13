using System;
using Unit4.Automation;
using NUnit.Framework;
using Unit4.Automation.Model;
using Unit4.Automation.Interfaces;
using System.Linq;
using Unit4.Automation.Tests.Helpers;
using Criteria = Unit4.Automation.Tests.Helpers.A.Criteria;

namespace Unit4.Automation.Tests
{
    [TestFixture]
    internal class BcrFilterTests
    {
        [Test]
        public void GivenTierOption_ThenLinesNotMatchingThatTierShouldNotBeIncluded([Values] Criteria criteria)
        {
            var filter = A.BcrFilter().With(criteria, "tier").Build();

            var bcr = new Bcr(new BcrLine[] { A.BcrLine().With(criteria, "notTheRightTier") });

            Assert.That(filter.Use(bcr).Lines, Is.Empty);
        }

        [Test]
        public void GivenTierOption_ThenLinesMatchingThatTierShouldBeIncluded([Values] Criteria criteria)
        {
            var filter = A.BcrFilter().With(criteria, "tier").Build();

            var bcr = new Bcr(new BcrLine[] { A.BcrLine().With(criteria, "tier") });

            Assert.That(filter.Use(bcr).Lines.ToList(), Has.Count.EqualTo(1));
        }

        [Test]
        public void GivenNoTierOption_ThenAllLinesShouldBeIncluded([Values] Criteria criteria)
        {
            var filter = A.BcrFilter().Build();

            var bcr = new Bcr(new BcrLine[] { A.BcrLine().With(criteria, "tier") });

            Assert.That(filter.Use(bcr).Lines.ToList(), Has.Count.EqualTo(1));
        }

        [Test]
        public void GivenTierOptionWithMultipleValues_ThenLinesMatchingThatAnyOfThoseValuesShouldBeIncluded([Values] Criteria criteria)
        {
            var filter = A.BcrFilter().With(criteria, "firstTier2", "secondTier2").Build();

            var firstBcrLine = A.BcrLine().With(criteria, "firstTier2").Build();
            var secondBcrLine = A.BcrLine().With(criteria, "secondTier2").Build();
            var thirdBcrLine = A.BcrLine().With(criteria, "thirdTier2").Build();

            var bcr = new Bcr(new BcrLine[] { firstBcrLine, secondBcrLine, thirdBcrLine });

            Assert.That(filter.Use(bcr).Lines.ToList(), Is.EquivalentTo(new BcrLine[] { firstBcrLine, secondBcrLine }));
        }

        [TestCase(Criteria.Tier2)]
        [TestCase(Criteria.Tier3)]
        [TestCase(Criteria.Tier2, Criteria.Tier3)]
        public void GivenMatchOnTier2Only_ThenTheLineShouldBeIncluded(params Criteria[] criteria)
        {
            var filter = A.BcrFilter().WithTier2("1").WithTier3("1").Build();

            var tiers = (Criteria[])Enum.GetValues(typeof(Criteria));
            var blankLine = tiers.Aggregate(A.BcrLine(), (builder, t) => builder.With(t, "0"));
            
            var bcrLine = criteria.Aggregate(blankLine, (builder, t) => builder.With(t, "1")).Build();

            var bcr = new Bcr(new BcrLine[] { bcrLine });

            Assert.That(filter.Use(bcr).Lines.ToList(), Is.EquivalentTo(new BcrLine[] { bcrLine }));
        }

        [Test]
        public void GivenMatchOnNoTier_ThenTheLineShouldNotBeIncluded()
        {
            var filter = A.BcrFilter().WithTier2("1").WithTier3("1").Build();

            var firstBcrLine = A.BcrLine().WithTier2("0").WithTier3("0").Build();

            var bcr = new Bcr(new BcrLine[] { firstBcrLine });

            Assert.That(filter.Use(bcr).Lines.ToList(), Is.Empty);
        }
    }
}