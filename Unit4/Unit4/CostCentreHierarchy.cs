using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;

namespace Unit4
{
    internal class CostCentreHierarchy
    {
        private DataSet RunReport(string resql)
        {
            var credentials = new Credentials();

            var engine = new Unit4Engine(credentials);
            return engine.RunReport(resql);
        }

        public IEnumerable<string> GetTier3List()
        {
            var tier3List = new List<string>();

            var data = RunReport(Resql.GetCostCentreList);
            foreach (DataRow row in data.Tables[0].Rows)
            {
                var tier3 = row["r0r1dim_value"] as string;
                var tier3Name = row["xr0r1dim_value"] as string;
                var costCentre = row["dim_value"] as string;
                var costCentreName = row["xdim_value"] as string;
                if (costCentre.StartsWith("3000"))
                {
                    tier3List.Add(tier3);
                }
            }
            return tier3List.Distinct().Where(x => x.StartsWith("30T3"));
        }
    }
}