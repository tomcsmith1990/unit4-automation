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

            MSExcel.Worksheet excelWorkSheet = excelWorkBook.Sheets.Add() as MSExcel.Worksheet;
            excelWorkSheet.Name = Guid.NewGuid().ToString("N").Substring(0, 16);

            excelWorkSheet.Cells[1, 1] = "Tier1";
            excelWorkSheet.Cells[1, 2] = "Tier1";
            excelWorkSheet.Cells[1, 3] = "Tier2";
            excelWorkSheet.Cells[1, 4] = "Tier2";
            excelWorkSheet.Cells[1, 5] = "Tier3";
            excelWorkSheet.Cells[1, 6] = "Tier3";
            excelWorkSheet.Cells[1, 7] = "Tier4";
            excelWorkSheet.Cells[1, 8] = "Tier4";
            excelWorkSheet.Cells[1, 9] = "Cost Centre";
            excelWorkSheet.Cells[1, 10] = "Cost Centre";
            excelWorkSheet.Cells[1, 11] = "Account";
            excelWorkSheet.Cells[1, 12] = "Account";
            excelWorkSheet.Cells[1, 13] = "Budget";
            excelWorkSheet.Cells[1, 14] = "Profile";
            excelWorkSheet.Cells[1, 15] = "Actuals";
            excelWorkSheet.Cells[1, 16] = "Variance";
            excelWorkSheet.Cells[1, 17] = "Forecast";
            excelWorkSheet.Cells[1, 18] = "Outturn Variance";

            var i = 2;
            foreach (var row in data)
            {
                excelWorkSheet.Cells[i, 1] = row.Tier1;
                excelWorkSheet.Cells[i, 2] = row.Tier1Name;
                excelWorkSheet.Cells[i, 3] = row.Tier2;
                excelWorkSheet.Cells[i, 4] = row.Tier2Name;
                excelWorkSheet.Cells[i, 5] = row.Tier3;
                excelWorkSheet.Cells[i, 6] = row.Tier3Name;
                excelWorkSheet.Cells[i, 7] = row.Tier4;
                excelWorkSheet.Cells[i, 8] = row.Tier4Name;
                excelWorkSheet.Cells[i, 9] = row.CostCentre;
                excelWorkSheet.Cells[i, 10] = row.CostCentreName;
                excelWorkSheet.Cells[i, 11] = row.Account;
                excelWorkSheet.Cells[i, 12] = row.AccountName;
                excelWorkSheet.Cells[i, 13] = row.Budget;
                excelWorkSheet.Cells[i, 14] = row.Profile;
                excelWorkSheet.Cells[i, 15] = row.Actuals;
                excelWorkSheet.Cells[i, 16] = row.Variance;
                excelWorkSheet.Cells[i, 17] = row.Forecast;
                excelWorkSheet.Cells[i, 18] = row.OutturnVariance;
                i++;
            }

            excelWorkSheet.Range[excelWorkSheet.Cells[2, 13], excelWorkSheet.Cells[i - 1, 18]].NumberFormat = "#,##0.00_ ;[Red]-#,##0.00";

            excelWorkSheet.Range[excelWorkSheet.Cells[1,1], excelWorkSheet.Cells[i - 1, 18]].AutoFilter();

            excelWorkSheet.Columns.AutoFit();

            excelWorkBook.SaveAs(path);
            excelWorkBook.Close();
            excelApp.Quit();
        }
    }
}