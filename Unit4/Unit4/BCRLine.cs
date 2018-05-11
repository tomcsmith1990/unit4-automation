using System;
using System.Data;
using System.Collections.Generic;

namespace Unit4
{
    internal class BCRLine
    {
        public string Tier1 { get; set; }
        public string Tier2 { get; set; }
        public string Tier3 { get; set; }
        public string Tier4 { get; set; }
        public string CostCentre { get; set; }
        public string Account { get; set; }

        public string Tier1Name { get; set; }
        public string Tier2Name { get; set; }
        public string Tier3Name { get; set; }
        public string Tier4Name { get; set; }
        public string CostCentreName { get; set; }
        public string AccountName { get; set; }

        public double Budget { get; set; }
        public double Profile { get; set; }
        public double Actuals { get; set; }
        public double Variance { get; set; }
        public double Forecast { get; set; }
        public double OutturnVariance { get; set; }
    }
}