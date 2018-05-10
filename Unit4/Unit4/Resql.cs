using System;

namespace Unit4
{
    internal static class Resql
    {
        public static string BcrByCostCentre
        {
            get
            {
                return @".name [GL-BAL-001 : General Balances Monitoring Report]

.declare [Directorate (Tier1)] String ''

.declare [Service Group (Tier2)] String ''

.declare [Budget Group (Tier4)] String ''

.declare [Cost Centre] String '{0}'

.declare [Service (Tier3)] String ''

.declare [Account] String ''

.query [GL-BAL-001 : General Balances Monitoring Report] 
    agr_getBrowser 'GL-BAL-001 : General Balances Monitoring Report', r0r0r0r3dim2_eq='$?[Directorate (Tier1)]', r0r0r3dim2_eq='$?[Service Group (Tier2)]', r3dim2_eq='$?[Budget Group (Tier4)]', dim2_eq='$?[Cost Centre]', r0r3dim2_eq='$?[Service (Tier3)]', dim1_eq='$?[Account]'
.endQuery";
            }
        }

        public static string GetCostCentreList
        {
            get
            {
                return @".name [GL-COA-004 : Listing - Cost Centres]

.declare [Budget Group (Tier4)] String ''

.declare [Cost Centre] String ''

.query [GL-COA-004 : Listing - Cost Centres] 
    agr_getBrowser 'GL-COA-004 : Listing - Cost Centres', r4dim_value_eq='$?[Budget Group (Tier4)]', dim_value_eq='$?[Cost Centre]'
.endQuery";
            }
        }
    }
}