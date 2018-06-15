using System;
using Unit4.Automation;
using NUnit.Framework;
using Unit4.Automation.Model;
using Unit4.Automation.Interfaces;
using System.Linq;
using Unit4.Automation.Tests.Helpers;
using System.Collections.Generic;
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
            var filter = A.BcrFilter().With(criteria, "firstTier", "secondTier").Build();

            var firstBcrLine = A.BcrLine().With(criteria, "firstTier").Build();
            var secondBcrLine = A.BcrLine().With(criteria, "secondTier").Build();
            var thirdBcrLine = A.BcrLine().With(criteria, "thirdTier").Build();

            var bcr = new Bcr(new BcrLine[] { firstBcrLine, secondBcrLine, thirdBcrLine });

            Assert.That(filter.Use(bcr).Lines.ToList(), Is.EquivalentTo(new BcrLine[] { firstBcrLine, secondBcrLine }));
        }

        [Test]
        public void GivenMatchOnAtLeastOneTier_ThenTheLineShouldBeIncluded(
            [ValueSource("CriteriaPowerset")] IEnumerable<Criteria> criteria)
        {
            var tiers = (Criteria[])Enum.GetValues(typeof(Criteria));

            var filter = tiers.Aggregate(A.BcrFilter(), (builder, t) => builder.With(t, "1")).Build();

            var blankLine = tiers.Aggregate(A.BcrLine(), (builder, t) => builder.With(t, "0"));
            
            var bcrLine = criteria.Aggregate(blankLine, (builder, t) => builder.With(t, "1")).Build();

            var bcr = new Bcr(new BcrLine[] { bcrLine });

            Assert.That(filter.Use(bcr).Lines.ToList(), Is.EquivalentTo(new BcrLine[] { bcrLine }));
        }

        private static IEnumerable<IEnumerable<Criteria>> CriteriaPowerset
        {
            get
            {
                return Enum.GetValues(typeof(Criteria)).Cast<Criteria>().Powerset().Where(x => x.Any());
            }
        }

        [Test]
        public void GivenMatchOnNoTier_ThenTheLineShouldNotBeIncluded()
        {
            var allCriteria = (Criteria[])Enum.GetValues(typeof(Criteria));

            var filter = 
                allCriteria.Aggregate(A.BcrFilter(), (builder, c) => builder.With(c, "1")).Build();
            
            var bcrLine = 
                allCriteria.Aggregate(A.BcrLine(), (builder, c) => builder.With(c, "0")).Build();

            var bcr = new Bcr(new BcrLine[] { bcrLine });

            Assert.That(filter.Use(bcr).Lines.ToList(), Is.Empty);
        }
    }
}