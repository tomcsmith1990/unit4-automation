using System.Collections.Generic;
using System.Data;
using Unit4.Automation.Model;

namespace Unit4.Automation.Commands.BcrCommand
{
    internal class BcrLineBuilder
    {
        public IEnumerable<BcrLine> Build(DataSet data)
        {
            foreach (DataRow row in data.Tables[0].Rows)
            {
                yield return new BcrLine()
                {
                    CostCentre = new CostCentre()
                    {
                        Tier1 = row["r0r0r0r3dim2"] as string,
                        Tier2 = row["r0r0r3dim2"] as string,
                        Tier3 = row["r0r3dim2"] as string,
                        Tier4 = row["r3dim2"] as string,
                        Code = row["dim2"] as string,

                        Tier1Name = row["xr0r0r0r3dim2"] as string,
                        Tier2Name = row["xr0r0r3dim2"] as string,
                        Tier3Name = row["xr0r3dim2"] as string,
                        Tier4Name = row["xr3dim2"] as string,
                        CostCentreName = row["xdim2"] as string
                    },

                    Account = row["dim1"] as string,
                    AccountName = row["xdim1"] as string,

                    Budget = (double) row["plb_amount"],
                    Profile = (double) row["f0_budget_to_da13"],
                    Actuals = (double) row["f1_total_exp_to16"],
                    Variance = (double) row["f3_variance_to_15"],
                    Forecast = (double) row["plf_amount"],
                    OutturnVariance = (double) row["f2_outturn_vari18"]
                };
            }
        }
    }
}