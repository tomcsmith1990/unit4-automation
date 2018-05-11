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

            AddHeader(sheet, 1);

            var rowToStartData = 2;

            Parallel.For(0, data.Count(), new ParallelOptions { MaxDegreeOfParallelism = 3 }, i =>
            {
                var rowNumber = i + rowToStartData;
                AddRow(sheet, rowNumber, data.ElementAt(i));
            });

            SetNumberFormat(sheet.Range[sheet.Cells[rowToStartData, 13], sheet.Cells[rowToStartData + data.Count() - 1, 18]]);

            AutoFilter(sheet.Range[sheet.Cells[1, 1], sheet.Cells[rowToStartData + data.Count() - 1, 18]]);
            sheet.Columns.AutoFit();

            workbook.SaveAs(path);
            workbook.Close();
            app.Quit();
        }

        private void AddHeader(MSExcel.Worksheet sheet, int headerRow)
        {
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
        }

        private void AddRow(MSExcel.Worksheet sheet, int rowNumber, BCRLine line)
        {
            sheet.Cells[rowNumber, 1] = line.Tier1;
            sheet.Cells[rowNumber, 2] = line.Tier1Name;
            sheet.Cells[rowNumber, 3] = line.Tier2;
            sheet.Cells[rowNumber, 4] = line.Tier2Name;
            sheet.Cells[rowNumber, 5] = line.Tier3;
            sheet.Cells[rowNumber, 6] = line.Tier3Name;
            sheet.Cells[rowNumber, 7] = line.Tier4;
            sheet.Cells[rowNumber, 8] = line.Tier4Name;
            sheet.Cells[rowNumber, 9] = line.CostCentre;
            sheet.Cells[rowNumber, 10] = line.CostCentreName;
            sheet.Cells[rowNumber, 11] = line.Account;
            sheet.Cells[rowNumber, 12] = line.AccountName;
            sheet.Cells[rowNumber, 13] = line.Budget;
            sheet.Cells[rowNumber, 14] = line.Profile;
            sheet.Cells[rowNumber, 15] = line.Actuals;
            sheet.Cells[rowNumber, 16] = line.Variance;
            sheet.Cells[rowNumber, 17] = line.Forecast;
            sheet.Cells[rowNumber, 18] = line.OutturnVariance;
        }

        private void SetNumberFormat(MSExcel.Range range)
        {
            range.NumberFormat = "#,##0.00_ ;[Red]-#,##0.00";
        }

        private void AutoFilter(MSExcel.Range range)
        {
            range.AutoFilter(1);
        }
    }
}