using System;
using Unit4.Automation.Model;
using Criteria = Unit4.Automation.Tests.Helpers.A.Criteria;
using Unit4.Automation.Commands.BcrCommand;

namespace Unit4.Automation.Tests.Helpers
{
    internal class BcrFilterBuilder
    {
        private string[] _tier1;
        private string[] _tier2;
        private string[] _tier3;
        private string[] _tier4;
        private string[] _costCentre;

        public BcrFilterBuilder With(Criteria criteria, params string[] value)
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

        public BcrFilter Build()
        {
            return this;
        }

        public static implicit operator BcrFilter(BcrFilterBuilder builder)
        {
            return new BcrFilter(new BcrOptions(builder._tier1, builder._tier2, builder._tier3, builder._tier4, builder._costCentre, null));
        }
    }
}