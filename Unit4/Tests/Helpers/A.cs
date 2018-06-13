using System;
using Unit4.Automation.Model;
using System.Collections.Generic;

namespace Unit4.Automation.Tests.Helpers
{
    internal static class A
    {
        public enum Criteria { Tier2, Tier3 }

        public static BcrLineBuilder BcrLine()
        {
            return new BcrLineBuilder();
        }

        public static BcrFilterBuilder BcrFilter()
        {
            return new BcrFilterBuilder();
        }

        public static IEnumerable<string> ValueOf(this BcrOptions options, Criteria criteria)
        {
            switch (criteria)
            {
                case Criteria.Tier2: return options.Tier2;
                case Criteria.Tier3: return options.Tier3;
                default: throw new NotSupportedException(criteria.ToString());
            }
        }

        public static string Name(this Criteria criteria)
        {
            switch (criteria)
            {
                case Criteria.Tier2: return "tier2";
                case Criteria.Tier3: return "tier3";
                default: throw new NotSupportedException(criteria.ToString());
            }
        }
    }
}