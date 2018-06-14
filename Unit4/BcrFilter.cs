using System;
using Unit4.Automation.Model;
using Unit4.Automation.Interfaces;
using System.Linq;

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
            if (_options.Tier2 == null)
            {
                return bcr;
            }

            return new Bcr(bcr.Lines.Where(x => Matches(x.CostCentre)).ToList());
        }

        private bool Matches(CostCentre costCentre)
        {
            if (!HasTier2 && !HasTier3)
            {
                return true;
            }

            var matchesTier2 = HasTier2 && MatchesTier2(costCentre);
            var matchesTier3 = HasTier3 && MatchesTier3(costCentre);
            return matchesTier2 || matchesTier3;
        }

        private bool HasTier2 { get { return _options.Tier2 != null && _options.Tier2.Any(); } }
        private bool HasTier3 { get { return _options.Tier3 != null && _options.Tier3.Any(); } }

        private bool MatchesTier2(CostCentre costCentre)
        {
            return  _options.Tier2.Any(x => string.Equals(x, costCentre.Tier2));
        }

        private bool MatchesTier3(CostCentre costCentre)
        {
            return _options.Tier3.Any(x => string.Equals(x, costCentre.Tier3));
        }
    }
}