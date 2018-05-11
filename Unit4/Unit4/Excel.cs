using System;
using System.Data;
using MSExcel = Microsoft.Office.Interop.Excel;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Unit4
{
    internal class Excel
    {
        public void WriteToExcel(string path, IEnumerable<BCRLine> data)
        {
            var app = new MSExcel.Application();
            var workbook = app.Workbooks.Add();

            var sheet = workbook.Sheets.Add() as MSExcel.Worksheet;
            sheet.Name = Guid.NewGuid().ToString("N").Substring(0, 16);

            var headerRow = 1;

            sheet.Cells[headerRow, 1] = "Tier1";
            sheet.Cells[headerRow, 2] = "Tier1";
            sheet.Cells[headerRow, 3] = "Tier2";
            sheet.Cells[headerRow, 4] = "Tier2";
            sheet.Cells[headerRow, 5] = "Tier3";
            sheet.Cells[headerRow, 6] = "Tier3";
            sheet.Cells[headerRow, 7] = "Tier4";
            sheet.Cells[headerRow, 8] = "Tier4";
            sheet.Cells[headerRow, 9] = "Cost Centre";
            sheet.Cells[headerRow, 10] = "Cost Centre";
            sheet.Cells[headerRow, 11] = "Account";
            sheet.Cells[headerRow, 12] = "Account";
            sheet.Cells[headerRow, 13] = "Budget";
            sheet.Cells[headerRow, 14] = "Profile";
            sheet.Cells[headerRow, 15] = "Actuals";
            sheet.Cells[headerRow, 16] = "Variance";
            sheet.Cells[headerRow, 17] = "Forecast";
            sheet.Cells[headerRow, 18] = "Outturn Variance";

            var rowToStartData = 2;

            Parallel.For(0, data.Count(), new ParallelOptions { MaxDegreeOfParallelism = 3 }, i =>
            {
                var row = data.ElementAt(i);

                sheet.Cells[i + rowToStartData, 1] = row.Tier1;
                sheet.Cells[i + rowToStartData, 2] = row.Tier1Name;
                sheet.Cells[i + rowToStartData, 3] = row.Tier2;
                sheet.Cells[i + rowToStartData, 4] = row.Tier2Name;
                sheet.Cells[i + rowToStartData, 5] = row.Tier3;
                sheet.Cells[i + rowToStartData, 6] = row.Tier3Name;
                sheet.Cells[i + rowToStartData, 7] = row.Tier4;
                sheet.Cells[i + rowToStartData, 8] = row.Tier4Name;
                sheet.Cells[i + rowToStartData, 9] = row.CostCentre;
                sheet.Cells[i + rowToStartData, 10] = row.CostCentreName;
                sheet.Cells[i + rowToStartData, 11] = row.Account;
                sheet.Cells[i + rowToStartData, 12] = row.AccountName;
                sheet.Cells[i + rowToStartData, 13] = row.Budget;
                sheet.Cells[i + rowToStartData, 14] = row.Profile;
                sheet.Cells[i + rowToStartData, 15] = row.Actuals;
                sheet.Cells[i + rowToStartData, 16] = row.Variance;
                sheet.Cells[i + rowToStartData, 17] = row.Forecast;
                sheet.Cells[i + rowToStartData, 18] = row.OutturnVariance;
            });

            sheet.Range[sheet.Cells[rowToStartData, 13], sheet.Cells[rowToStartData + data.Count() - 1, 18]].NumberFormat = "#,##0.00_ ;[Red]-#,##0.00";

            sheet.Range[sheet.Cells[1, 1], sheet.Cells[rowToStartData + data.Count() - 1, 18]].AutoFilter(1);
            sheet.Columns.AutoFit();

            workbook.SaveAs(path);
            workbook.Close();
            app.Quit();
        }
    }
}