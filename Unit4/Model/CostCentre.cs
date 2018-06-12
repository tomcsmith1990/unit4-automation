using System;
using System.Linq;

namespace Unit4.Automation.Model
{
    internal class CostCentre
    {
        public string Tier1 { get; set; }
        public string Tier2 { get; set; }
        public string Tier3 { get; set; }
        public string Tier4 { get; set; }
        public string Code { get; set; }

        public string Tier1Name { get; set; }
        public string Tier2Name { get; set; }
        public string Tier3Name { get; set; }
        public string Tier4Name { get; set; }
        public string CostCentreName { get; set; }

        public bool Matches(BcrOptions options)
        {
            return options.Tier2.Any(x => string.Equals(x, Tier2));
        }
    }
}