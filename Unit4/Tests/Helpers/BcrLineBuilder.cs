using System;
using Unit4.Automation.Model;
using Criteria = Unit4.Automation.Tests.Helpers.A.Criteria;

namespace Unit4.Automation.Tests.Helpers
{
    internal class BcrLineBuilder
    {
        private string _tier2;
        private string _tier3;
        private string _tier4;
        
        public BcrLineBuilder With(Criteria criteria, string value)
        {
            switch (criteria)
            {
                case Criteria.Tier2: return WithTier2(value);
                case Criteria.Tier3: return WithTier3(value);
                case Criteria.Tier4: return WithTier4(value);
                default: throw new NotSupportedException(criteria.ToString());
            }
        }

        public BcrLineBuilder WithTier2(string tier2)
        {
            _tier2 = tier2;
            return this;
        }

        public BcrLineBuilder WithTier3(string tier3)
        {
            _tier3 = tier3;
            return this;
        }

        public BcrLineBuilder WithTier4(string tier4)
        {
            _tier4 = tier4;
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
                    Tier2 = builder._tier2,
                    Tier3 = builder._tier3,
                    Tier4 = builder._tier4,
                }
            };
        }
    }
}