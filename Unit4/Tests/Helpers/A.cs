using System;
using Unit4.Automation.Model;

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
    }
}