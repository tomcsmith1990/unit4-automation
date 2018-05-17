using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using Unit4.Automation.Interfaces;
using Unit4.Automation.Model;
using Unit4.Automation.ReportEngine;

namespace Unit4.Automation
{
    internal class CostCentreList : ICostCentres
    {
        private DataSet RunReport(string resql)
        {
            var credentials = new Credentials();

            var engine = new Unit4Engine(credentials);
            return engine.RunReport(resql);
        }

        public IEnumerable<CostCentre> GetCostCentres()
        {
            var data = RunReport(Resql.GetCostCentreList);
            return data.Tables[0].Rows.Cast<DataRow>().Select(CreateCostCentre);
        }

        private CostCentre CreateCostCentre(DataRow row)
        {
            return new CostCentre()
            {
                Tier1 = row["r0r0r0r1dim_value"] as string,
                Tier2 = row["r0r0r1dim_value"] as string,
                Tier3 = row["r0r1dim_value"] as string,
                Tier4 = row["r1dim_value"] as string,
                Code = row["dim_value"] as string,

                Tier1Name = row["xr0r0r0r1dim_value"] as string,
                Tier2Name = row["xr0r0r1dim_value"] as string,
                Tier3Name = row["xr0r1dim_value"] as string,
                Tier4Name = row["xr1dim_value"] as string,
                CostCentreName = row["xdim_value"] as string
            };
        }
    }
}