using System;
using Unit4.Automation.Model;
using Criteria = Unit4.Automation.Tests.Helpers.A.Criteria;

namespace Unit4.Automation.Tests.Helpers
{
    internal class BcrFilterBuilder
    {
        private string[] _tier2;
        private string[] _tier3;
        private string[] _tier4;

        public BcrFilterBuilder With(Criteria criteria, params string[] value)
        {
            switch (criteria)
            {
                case Criteria.Tier2: return WithTier2(value);
                case Criteria.Tier3: return WithTier3(value);
                case Criteria.Tier4: return WithTier4(value);
                default: throw new NotSupportedException(criteria.ToString());
            }
        }
        public BcrFilterBuilder WithTier2(params string[] tier2)
        {
            _tier2 = tier2;
            return this;
        }

        public BcrFilterBuilder WithTier3(params string[] tier3)
        {
            _tier3 = tier3;
            return this;
        }

        public BcrFilterBuilder WithTier4(params string[] tier4)
        {
            _tier4 = tier4;
            return this;
        }

        public BcrFilter Build()
        {
            return (BcrFilter)this;
        }

        public static implicit operator BcrFilter(BcrFilterBuilder builder)
        {
            return new BcrFilter(new BcrOptions(builder._tier2, builder._tier3, builder._tier4));
        }
    }
}