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
            if (!HasOption(_options.Tier1) && !HasOption(_options.Tier2) && !HasOption(_options.Tier3) && !HasOption(_options.Tier4))
            {
                return true;
            }

            var matchesTier1 = HasOption(_options.Tier1) && MatchesTier1(costCentre);
            var matchesTier2 = HasOption(_options.Tier2) && MatchesTier2(costCentre);
            var matchesTier3 = HasOption(_options.Tier3) && MatchesTier3(costCentre);
            var matchesTier4 = HasOption(_options.Tier4) && MatchesTier4(costCentre);
            return matchesTier1 || matchesTier2 || matchesTier3 || matchesTier4;
        }

        private bool HasOption(IEnumerable<string> option)
        {
            return option != null && option.Any();
        }

        private bool MatchesTier1(CostCentre costCentre)
        {
            return  _options.Tier1.Any(x => string.Equals(x, costCentre.Tier1));
        }

        private bool MatchesTier2(CostCentre costCentre)
        {
            return  _options.Tier2.Any(x => string.Equals(x, costCentre.Tier2));
        }

        private bool MatchesTier3(CostCentre costCentre)
        {
            return _options.Tier3.Any(x => string.Equals(x, costCentre.Tier3));
        }

        private bool MatchesTier4(CostCentre costCentre)
        {
            return _options.Tier4.Any(x => string.Equals(x, costCentre.Tier4));
        }
    }
}