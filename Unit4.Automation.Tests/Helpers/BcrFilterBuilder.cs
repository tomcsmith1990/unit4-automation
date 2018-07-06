using System;
using Unit4.Automation.Commands.BcrCommand;
using Unit4.Automation.Model;

namespace Unit4.Automation.Tests.Helpers
{
    internal class BcrFilterBuilder
    {
        private string[] _costCentre;
        private string[] _tier1;
        private string[] _tier2;
        private string[] _tier3;
        private string[] _tier4;

        public BcrFilterBuilder With(A.Criteria criteria, params string[] value)
        {
            switch (criteria)
            {
                case A.Criteria.Tier1:
                    _tier1 = value;
                    break;
                case A.Criteria.Tier2:
                    _tier2 = value;
                    break;
                case A.Criteria.Tier3:
                    _tier3 = value;
                    break;
                case A.Criteria.Tier4:
                    _tier4 = value;
                    break;
                case A.Criteria.CostCentre:
                    _costCentre = value;
                    break;
                default: throw new NotSupportedException(criteria.ToString());
            }

            return this;
        }

        public BcrFilter Build() => this;

        public static implicit operator BcrFilter(BcrFilterBuilder builder)
        {
            return new BcrFilter(new BcrOptions(builder._tier1, builder._tier2, builder._tier3, builder._tier4,
                builder._costCentre, null, false));
        }
    }
}