using System;
using System.Data;
using Unit4.Automation.Model;
using System.Linq;

namespace Unit4.Automation.Tests.Helpers
{
    internal class BcrDataSetBuilder
    {
        public static DataSet Build(params CostCentre[] costCentres)
        {
            return Build(costCentres.Select(x => new BcrLine() { CostCentre = x }).ToArray());
        }

        private static DataSet Build(params BcrLine[] lines)
        {
            var dataset = new DataSet();
            var table = dataset.Tables.Add("foo");
            table.Columns.Add("r0r0r0r3dim2", typeof(string));
            table.Columns.Add("r0r0r3dim2", typeof(string));
            table.Columns.Add("r0r3dim2", typeof(string));
            table.Columns.Add("r3dim2", typeof(string));
            table.Columns.Add("dim2", typeof(string));
            table.Columns.Add("xr0r0r0r3dim2", typeof(string));
            table.Columns.Add("xr0r0r3dim2", typeof(string));
            table.Columns.Add("xr0r3dim2", typeof(string));
            table.Columns.Add("xr3dim2", typeof(string));
            table.Columns.Add("xdim2", typeof(string));
            table.Columns.Add("dim1", typeof(string));
            table.Columns.Add("xdim1", typeof(string));
            table.Columns.Add("plb_amount", typeof(double));
            table.Columns.Add("f0_budget_to_da13", typeof(double));
            table.Columns.Add("f1_total_exp_to16", typeof(double));
            table.Columns.Add("f3_variance_to_15", typeof(double));
            table.Columns.Add("plf_amount", typeof(double));
            table.Columns.Add("f2_outturn_vari18", typeof(double));

            foreach (var line in lines)
            {
                var row = table.NewRow();
                var code = line.CostCentre;
                table.Rows.Add(code.Tier1, code.Tier2, code.Tier3, code.Tier3, code.Code, code.Tier1Name, code.Tier2Name, code.Tier3Name, code.Tier4Name, code.CostCentreName, line.Account, line.AccountName, line.Budget, line.Profile, line.Actuals, line.Variance, line.Forecast, line.OutturnVariance);
            }
            return dataset;
        }
    }
}