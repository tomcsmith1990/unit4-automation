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
            return MatchesTier2(costCentre);
        }

        private bool MatchesTier2(CostCentre costCentre)
        {
            return _options.Tier2 == null || !_options.Tier2.Any() || _options.Tier2.Any(x => string.Equals(x, costCentre.Tier2));
        }
    }
}