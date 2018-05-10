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

        public static IEnumerable<BCRLine> FromDataSet(DataSet data)
        {
            foreach (DataRow row in data.Tables[0].Rows)
            {
                yield return new BCRLine() {
                    Tier1 = row["r0r0r0r3dim2"] as string,
                    Tier2 = row["r0r0r3dim2"] as string,
                    Tier3 = row["r0r3dim2"] as string,
                    Tier4 = row["r3dim2"] as string,
                    CostCentre = row["dim2"] as string,
                    Account = row["dim1"] as string,

                    Tier1Name = row["xr0r0r0r3dim2"] as string,
                    Tier2Name = row["xr0r0r3dim2"] as string,
                    Tier3Name = row["xr0r3dim2"] as string,
                    Tier4Name = row["xr3dim2"] as string,
                    CostCentreName = row["xdim2"] as string,
                    AccountName = row["xdim1"] as string,

                    Budget = (double) row["plb_amount"] ,
                    Profile = (double) row["f0_budget_to_da13"] ,
                    Actuals = (double) row["f1_total_exp_to16"] ,
                    Variance = (double) row["f3_variance_to_15"] ,
                    Forecast = (double) row["plf_amount"] ,
                    OutturnVariance = (double) row["f2_outturn_vari18"] 
                };
            }
    }
    }
}