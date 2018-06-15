using Unit4.Automation.Model;
using Unit4.Automation.Interfaces;
using System.Linq;
using System.Collections.Generic;

namespace Unit4.Automation
{
    internal class BcrFilter : IBcrMiddleware
    {
        private readonly BcrOptions _options;

        public BcrFilter(BcrOptions options)
        {
            _options = options;
        }

        public Bcr Use(Bcr bcr)
        {
            return new Bcr(bcr.Lines.Where(x => Matches(x.CostCentre)).ToList());
        }

        private bool Matches(CostCentre costCentre)
        {
            if (!HasOption(_options.Tier1) && !HasOption(_options.Tier2) && !HasOption(_options.Tier3) && !HasOption(_options.Tier4) && !HasOption(_options.CostCentre))
            {
                return true;
            }

            var matchesTier1 = HasOption(_options.Tier1) && Matches(_options.Tier1, costCentre.Tier1);
            var matchesTier2 = HasOption(_options.Tier2) && Matches(_options.Tier2, costCentre.Tier2);
            var matchesTier3 = HasOption(_options.Tier3) && Matches(_options.Tier3, costCentre.Tier3);
            var matchesTier4 = HasOption(_options.Tier4) && Matches(_options.Tier4, costCentre.Tier4);
            var matchesCostCentre = HasOption(_options.CostCentre) && Matches(_options.CostCentre, costCentre.Code);
            return matchesTier1 || matchesTier2 || matchesTier3 || matchesTier4 || matchesCostCentre;
        }

        private bool HasOption(IEnumerable<string> option)
        {
            return option != null && option.Any();
        }

        private bool Matches(IEnumerable<string> options, string value)
        {
            return options.Any(x => string.Equals(x, value));
        }
    }
}