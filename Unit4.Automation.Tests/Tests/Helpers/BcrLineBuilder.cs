using System;
using Unit4.Automation.Model;
using Criteria = Unit4.Automation.Tests.Helpers.A.Criteria;

namespace Unit4.Automation.Tests.Helpers
{
    internal class BcrLineBuilder
    {
        private string _tier1;
        private string _tier2;
        private string _tier3;
        private string _tier4;
        private string _costCentre;
        
        public BcrLineBuilder With(Criteria criteria, string value)
        {
            switch (criteria)
            {
                case Criteria.Tier1: _tier1 = value; break;
                case Criteria.Tier2: _tier2 = value; break;
                case Criteria.Tier3: _tier3 = value; break;
                case Criteria.Tier4: _tier4 = value; break;
                case Criteria.CostCentre: _costCentre = value; break;
                default: throw new NotSupportedException(criteria.ToString());
            }

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
                    Tier1 = builder._tier1,
                    Tier2 = builder._tier2,
                    Tier3 = builder._tier3,
                    Tier4 = builder._tier4,
                    Code = builder._costCentre,
                }
            };
        }
    }
}