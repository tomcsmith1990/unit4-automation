using System;
using Unit4.Automation.Model;
using System.Collections.Generic;

namespace Unit4.Automation.Tests.Helpers
{
    internal static class A
    {
        public enum Criteria { Tier1, Tier2, Tier3, Tier4, CostCentre }

        public static BcrLineBuilder BcrLine() => new BcrLineBuilder();

        public static BcrFilterBuilder BcrFilter() => new BcrFilterBuilder();

        public static IEnumerable<string> ValueOf(this BcrOptions options, Criteria criteria)
        {
            switch (criteria)
            {
                case Criteria.Tier1: return options.Tier1;
                case Criteria.Tier2: return options.Tier2;
                case Criteria.Tier3: return options.Tier3;
                case Criteria.Tier4: return options.Tier4;
                case Criteria.CostCentre: return options.CostCentre;
                default: throw new NotSupportedException(criteria.ToString());
            }
        }

        public static string Name(this Criteria criteria)
        {
            switch (criteria)
            {
                case Criteria.Tier1: return "tier1";
                case Criteria.Tier2: return "tier2";
                case Criteria.Tier3: return "tier3";
                case Criteria.Tier4: return "tier4";
                case Criteria.CostCentre: return "costcentre";
                default: throw new NotSupportedException(criteria.ToString());
            }
        }
    }
}