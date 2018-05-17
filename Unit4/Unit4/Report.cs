using System;
using System.Linq;
using System.Collections.Generic;
using Tier = BcrReport.Tier;

namespace Unit4
{
    internal class Report
    {
        public Tier Tier { get; set; }
        public IGrouping<string, CostCentre> Hierarchy { get; set; }

        public string Parameter { get { return Hierarchy.Key; } }

        public bool ShouldFallBack { get { return (Tier == Tier.Tier3 || Tier == Tier.Tier4) && Hierarchy.Any(); } }

        public IEnumerable<Report> FallbackReports()
        {
            var fallbackGroups = Hierarchy.GroupBy(FallBackGroupingFunction(Tier), x => x);

            return fallbackGroups.Select(x => new Report() { Tier = FallBackTier(Tier), Hierarchy = x });
        }

        private Tier FallBackTier(Tier current)
        {
            switch (current)
            {
                case Tier.Tier3: return Tier.Tier4;
                case Tier.Tier4: return Tier.CostCentre;
                default: throw new InvalidOperationException("Should not fall back");
            }
        }

        private Func<CostCentre, string> FallBackGroupingFunction(Tier current)
        {
            switch (current)
            {
                case Tier.Tier3: return x => x.Tier4;
                case Tier.Tier4: return x => x.Code;
                default: throw new InvalidOperationException("Should not fall back");
            }
        }
    }
}