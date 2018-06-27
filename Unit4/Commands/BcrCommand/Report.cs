using System;
using System.Linq;
using System.Collections.Generic;
using Tier = Unit4.Automation.Commands.BcrCommand.BcrReport.Tier;
using Unit4.Automation.Model;

namespace Unit4.Automation.Commands.BcrCommand
{
    internal class Report
    {
        private readonly Tier _tier;
        private readonly IGrouping<string, CostCentre> _hierarchy;

        public Report(Tier tier, IGrouping<string, CostCentre> hierarchy)
        {
            _tier = tier;
            _hierarchy = hierarchy;    
        }

        public Tier Tier { get { return _tier; } }
        public IGrouping<string, CostCentre> Hierarchy { get { return _hierarchy; } }
        public string Parameter { get { return Hierarchy.Key; } }

        public bool ShouldFallBack { get { return (Tier == Tier.Tier3 || Tier == Tier.Tier4) && Hierarchy.Any(); } }

        public IEnumerable<Report> FallbackReports()
        {
            var fallbackGroups = Hierarchy.GroupBy(FallBackGroupingFunction(Tier), x => x);

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