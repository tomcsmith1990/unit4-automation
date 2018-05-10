using System;
using System.Data;
using MSExcel = Microsoft.Office.Interop.Excel;
using System.Collections.Generic;

namespace Unit4
{
    internal class Excel
    {
        public void WriteToExcel(string path, IEnumerable<BCRLine> data)
        {
            var excelApp = new MSExcel.Application();
            var excelWorkBook = excelApp.Workbooks.Add();

            MSExcel.Worksheet sheet = excelWorkBook.Sheets.Add() as MSExcel.Worksheet;
            sheet.Name = Guid.NewGuid().ToString("N").Substring(0, 16);

            sheet.Cells[1, 1] = "Tier1";
            sheet.Cells[1, 2] = "Tier1";
            sheet.Cells[1, 3] = "Tier2";
            sheet.Cells[1, 4] = "Tier2";
            sheet.Cells[1, 5] = "Tier3";
            sheet.Cells[1, 6] = "Tier3";
            sheet.Cells[1, 7] = "Tier4";
            sheet.Cells[1, 8] = "Tier4";
            sheet.Cells[1, 9] = "Cost Centre";
            sheet.Cells[1, 10] = "Cost Centre";
            sheet.Cells[1, 11] = "Account";
            sheet.Cells[1, 12] = "Account";
            sheet.Cells[1, 13] = "Budget";
            sheet.Cells[1, 14] = "Profile";
            sheet.Cells[1, 15] = "Actuals";
            sheet.Cells[1, 16] = "Variance";
            sheet.Cells[1, 17] = "Forecast";
            sheet.Cells[1, 18] = "Outturn Variance";

            var i = 2;
            foreach (var row in data)
            {
                sheet.Cells[i, 1] = row.Tier1;
                sheet.Cells[i, 2] = row.Tier1Name;
                sheet.Cells[i, 3] = row.Tier2;
                sheet.Cells[i, 4] = row.Tier2Name;
                sheet.Cells[i, 5] = row.Tier3;
                sheet.Cells[i, 6] = row.Tier3Name;
                sheet.Cells[i, 7] = row.Tier4;
                sheet.Cells[i, 8] = row.Tier4Name;
                sheet.Cells[i, 9] = row.CostCentre;
                sheet.Cells[i, 10] = row.CostCentreName;
                sheet.Cells[i, 11] = row.Account;
                sheet.Cells[i, 12] = row.AccountName;
                sheet.Cells[i, 13] = row.Budget;
                sheet.Cells[i, 14] = row.Profile;
                sheet.Cells[i, 15] = row.Actuals;
                sheet.Cells[i, 16] = row.Variance;
                sheet.Cells[i, 17] = row.Forecast;
                sheet.Cells[i, 18] = row.OutturnVariance;
                i++;
            }

            sheet.Range[sheet.Cells[2, 13], sheet.Cells[i - 1, 18]].NumberFormat = "#,##0.00_ ;[Red]-#,##0.00";

            sheet.Columns.AutoFit();

            excelWorkBook.SaveAs(path);
            excelWorkBook.Close();
            excelApp.Quit();
        }
    }
}