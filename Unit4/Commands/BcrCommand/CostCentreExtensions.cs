using Unit4.Automation.Model;
using System.Linq;
using System.Collections.Generic;

namespace Unit4.Automation.Commands.BcrCommand
{
    internal static class CostCentreExtensions
    {
        public static bool Matches(this CostCentre costCentre, BcrOptions options)
        {
            if (!HasOption(options.Tier1) && !HasOption(options.Tier2) && !HasOption(options.Tier3) && !HasOption(options.Tier4) && !HasOption(options.CostCentre))
            {
                return true;
            }

            var matchesTier1 = HasOption(options.Tier1) && Matches(options.Tier1, costCentre.Tier1);
            var matchesTier2 = HasOption(options.Tier2) && Matches(options.Tier2, costCentre.Tier2);
            var matchesTier3 = HasOption(options.Tier3) && Matches(options.Tier3, costCentre.Tier3);
            var matchesTier4 = HasOption(options.Tier4) && Matches(options.Tier4, costCentre.Tier4);
            var matchesCostCentre = HasOption(options.CostCentre) && Matches(options.CostCentre, costCentre.Code);
            return matchesTier1 || matchesTier2 || matchesTier3 || matchesTier4 || matchesCostCentre;
        }

        private static bool HasOption(IEnumerable<string> option)
        {
            return option != null && option.Any();
        }

        private static bool Matches(IEnumerable<string> options, string value)
        {
            return options.Any(x => string.Equals(x, value));
        }
    }
}