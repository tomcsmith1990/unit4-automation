using System;

namespace Unit4
{
    internal static class Resql
    {
        public static string GetCostCentreList
        {
            get
            {
                return @".name [GL-COA-001 Revenue Cost Centre Hierarchy Report]

.declare [Cost Centre] String ''

.declare [Company] String '$CLIENT'

.declare [Directorate] String ''

.declare [Service Group (Tier2)] String ''

.declare [Service (Tier3)] String ''

.declare [Budget Group (Tier4)] String ''

.query [GL-COA-001 Revenue Cost Centre Hierarchy Report] 
    agr_getBrowser 'GL-COA-001 Revenue Cost Centre Hierarchy Report', dim_value_eq='$?[Cost Centre]', client_eq='$?[Company]', r0r0r0r1dim_value_eq='$?[Directorate]', r0r0r1dim_value_eq='$?[Service Group (Tier2)]', r0r1dim_value_eq='$?[Service (Tier3)]', r1dim_value_eq='$?[Budget Group (Tier4)]'
.endQuery";
            }
        }

        public static string Bcr(string tier3 = "", string tier4 = "", string costCentre = "")
        {
            return string.Format(@".name [GL-BAL-001 : General Balances Monitoring Report]

.declare [Directorate (Tier1)] String ''

.declare [Service Group (Tier2)] String ''

.declare [Budget Group (Tier4)] String '{1}'

.declare [Cost Centre] String '{2}'

.declare [Service (Tier3)] String '{0}'

.declare [Account] String ''

.query [GL-BAL-001 : General Balances Monitoring Report] 
    agr_getBrowser 'GL-BAL-001 : General Balances Monitoring Report', r0r0r0r3dim2_eq='$?[Directorate (Tier1)]', r0r0r3dim2_eq='$?[Service Group (Tier2)]', r3dim2_eq='$?[Budget Group (Tier4)]', dim2_eq='$?[Cost Centre]', r0r3dim2_eq='$?[Service (Tier3)]', dim1_eq='$?[Account]'
.endQuery", tier3, tier4, costCentre);
        }

        public static string BcrTier3(string tier3)
        {
            return Bcr(tier3: tier3);
        }

        public static string BcrTier4(string tier4)
        {
            return Bcr(tier4: tier4);
        }
    }
}