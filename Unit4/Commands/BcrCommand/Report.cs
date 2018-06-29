using System;
using System.Linq;
using System.Collections.Generic;
using Tier = Unit4.Automation.Commands.BcrCommand.BcrReport.Tier;
using Unit4.Automation.Model;

namespace Unit4.Automation.Commands.BcrCommand
{
    internal class Report
    {
        private readonly IGrouping<string, CostCentre> _hierarchy;

        public Report(Tier tier, IGrouping<string, CostCentre> hierarchy)
        {
            Tier = tier;
            _hierarchy = hierarchy;    
        }

        public Tier Tier { get; }

        public string Parameter => _hierarchy.Key;

        public bool ShouldFallBack => (Tier == Tier.Tier3 || Tier == Tier.Tier4) && _hierarchy.Any();

        public IEnumerable<Report> FallbackReports()
        {
            var fallbackGroups = _hierarchy.GroupBy(FallBackGroupingFunction(Tier), x => x);

            return fallbackGroups.Select(x => new Report(FallBackTier(Tier), x));
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