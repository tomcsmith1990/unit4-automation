using System;
using System.Collections.Generic;
using System.Linq;
using Unit4.Automation.Model;

namespace Unit4.Automation.Commands.BcrCommand
{
    internal class Report
    {
        private readonly IGrouping<string, CostCentre> _hierarchy;

        public Report(BcrReport.Tier tier, IGrouping<string, CostCentre> hierarchy)
        {
            Tier = tier;
            _hierarchy = hierarchy;
        }

        public BcrReport.Tier Tier { get; }

        public string Parameter => _hierarchy.Key;

        public bool ShouldFallBack =>
            (Tier == BcrReport.Tier.Tier3 || Tier == BcrReport.Tier.Tier4) && _hierarchy.Any();

        public IEnumerable<Report> FallbackReports()
        {
            var fallbackGroups = _hierarchy.GroupBy(FallBackGroupingFunction(Tier), x => x);

            return fallbackGroups.Select(x => new Report(FallBackTier(Tier), x));
        }

        private BcrReport.Tier FallBackTier(BcrReport.Tier current)
        {
            switch (current)
            {
                case BcrReport.Tier.Tier3: return BcrReport.Tier.Tier4;
                case BcrReport.Tier.Tier4: return BcrReport.Tier.CostCentre;
                default: throw new InvalidOperationException("Should not fall back");
            }
        }

        private Func<CostCentre, string> FallBackGroupingFunction(BcrReport.Tier current)
        {
            switch (current)
            {
                case BcrReport.Tier.Tier3: return x => x.Tier4;
                case BcrReport.Tier.Tier4: return x => x.Code;
                default: throw new InvalidOperationException("Should not fall back");
            }
        }
    }
}