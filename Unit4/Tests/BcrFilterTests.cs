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
        public enum Criteria { Tier2 }

        [TestCase(Criteria.Tier2)]
        public void GivenTierOption_ThenLinesNotMatchingThatTierShouldNotBeIncluded(Criteria criteria)
        {
            var filter = A.BcrFilter().With(criteria, "tier").Build();

            var bcr = new Bcr(new BcrLine[] { A.BcrLine().With(criteria, "notTheRightTier") });

            Assert.That(filter.Use(bcr).Lines, Is.Empty);
        }

        [Test]
        public void GivenTier2Option_ThenLinesMatchingThatTier2ShouldBeIncluded()
        {
            var filter = A.BcrFilter().WithTier2("tier2").Build();

            var bcr = new Bcr(new BcrLine[] { A.BcrLine().WithTier2("tier2") });

            Assert.That(filter.Use(bcr).Lines.ToList(), Has.Count.EqualTo(1));
        }

        [Test]
        public void GivenNoTier2Option_ThenAllLinesShouldBeIncluded()
        {
            var filter = A.BcrFilter().Build();

            var bcr = new Bcr(new BcrLine[] { A.BcrLine().WithTier2("tier2") });

            Assert.That(filter.Use(bcr).Lines.ToList(), Has.Count.EqualTo(1));
        }

        [Test]
        public void GivenTier2OptionWithMultipleValues_ThenLinesMatchingThatAnyOfThoseValuesShouldBeIncluded()
        {
            var filter = A.BcrFilter().WithTier2("firstTier2", "secondTier2").Build();

            var firstBcrLine = A.BcrLine().WithTier2("firstTier2").Build();
            var secondBcrLine = A.BcrLine().WithTier2("secondTier2").Build();
            var thirdBcrLine = A.BcrLine().WithTier2("thirdTier2").Build();

            var bcr = new Bcr(new BcrLine[] { firstBcrLine, secondBcrLine, thirdBcrLine });

            Assert.That(filter.Use(bcr).Lines.ToList(), Is.EquivalentTo(new BcrLine[] { firstBcrLine, secondBcrLine }));
        }

        private static class A
        {
            public static BcrLineBuilder BcrLine()
            {
                return new BcrLineBuilder();
            }

            public static BcrFilterBuilder BcrFilter()
            {
                return new BcrFilterBuilder();
            }
        }

        private class BcrLineBuilder
        {
            private string _tier2;
            
            public BcrLineBuilder With(Criteria criteria, string value)
            {
                switch (criteria)
                {
                    case Criteria.Tier2: return WithTier2(value);
                    default: throw new NotSupportedException(criteria.ToString());
                }
            }

            public BcrLineBuilder WithTier2(string tier2)
            {
                _tier2 = tier2;
                return this;
            }

            public BcrLine Build()
            {
                return (BcrLine)this;
            }

            public static implicit operator BcrLine(BcrLineBuilder builder)
            {
                return new BcrLine() {
                    CostCentre = new CostCentre() {
                        Tier2 = builder._tier2
                    }
                };
            }
        }

        private class BcrFilterBuilder
        {
            private string[] _tier2;

            public BcrFilterBuilder With(Criteria criteria, params string[] value)
            {
                switch (criteria)
                {
                    case Criteria.Tier2: return WithTier2(value);
                    default: throw new NotSupportedException(criteria.ToString());
                }
            }
            public BcrFilterBuilder WithTier2(params string[] tier2)
            {
                _tier2 = tier2;
                return this;
            }

            public BcrFilter Build()
            {
                return (BcrFilter)this;
            }

            public static implicit operator BcrFilter(BcrFilterBuilder builder)
            {
                return new BcrFilter(new BcrOptions(builder._tier2));
            }
        }
    }
}