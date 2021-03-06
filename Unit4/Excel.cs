using System;
using System.Linq;
using System.Threading.Tasks;
using Unit4.Automation.Interfaces;
using Unit4.Automation.Model;
using MSExcel = Microsoft.Office.Interop.Excel;

namespace Unit4.Automation
{
    internal class Excel : IBcrWriter
    {
        public void Write(string path, Bcr bcr)
        {
            var app = new MSExcel.Application();
            var workbook = app.Workbooks.Add();

            var sheet = workbook.Sheets[1] as MSExcel.Worksheet;
            sheet.Name = Guid.NewGuid().ToString("N").Substring(0, 16);

            var headerRow = 2;
            AddHeader(sheet, headerRow);

            var rowToStartData = headerRow + 1;

            var data = bcr.Lines.ToList();

            Parallel.For(
                0,
                data.Count,
                new ParallelOptions { MaxDegreeOfParallelism = 3 },
                i =>
                {
                    var rowNumber = i + rowToStartData;
                    AddRow(sheet, rowNumber, data.ElementAt(i));
                });

            SetNumberFormat(
                sheet.Range[sheet.Cells[rowToStartData, 13], sheet.Cells[rowToStartData + data.Count - 1, 18]]);

            var lastRow = rowToStartData + data.Count - 1;
            AutoFilter(sheet.Range[sheet.Cells[headerRow, 1], sheet.Cells[lastRow, 18]]);

            AddSubtotals(sheet, 1, rowToStartData, lastRow);

            sheet.Columns.AutoFit();

            workbook.SaveAs(path);
            workbook.Close();
            app.Quit();
        }

        private void AddSubtotals(MSExcel.Worksheet sheet, int totalRow, int startRow, int endRow)
        {
            ((MSExcel.Range) sheet.Cells[totalRow, 13]).FormulaR1C1 = $"=SUBTOTAL(109, R{startRow}C:R{endRow}C";
            ((MSExcel.Range) sheet.Cells[totalRow, 14]).FormulaR1C1 = $"=SUBTOTAL(109, R{startRow}C:R{endRow}C";
            ((MSExcel.Range) sheet.Cells[totalRow, 15]).FormulaR1C1 = $"=SUBTOTAL(109, R{startRow}C:R{endRow}C";
            ((MSExcel.Range) sheet.Cells[totalRow, 16]).FormulaR1C1 = $"=SUBTOTAL(109, R{startRow}C:R{endRow}C";
            ((MSExcel.Range) sheet.Cells[totalRow, 17]).FormulaR1C1 = $"=SUBTOTAL(109, R{startRow}C:R{endRow}C";
            ((MSExcel.Range) sheet.Cells[totalRow, 18]).FormulaR1C1 = $"=SUBTOTAL(109, R{startRow}C:R{endRow}C";

            SetNumberFormat(sheet.Range[sheet.Cells[totalRow, 13], sheet.Cells[totalRow, 18]]);
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

            sheet.Range[sheet.Cells[headerRow, 1], sheet.Cells[headerRow, 18]].Interior.Color =
                MSExcel.XlRgbColor.rgbCornflowerBlue;
            sheet.Range[sheet.Cells[headerRow, 1], sheet.Cells[headerRow, 18]].Font.Bold = true;
        }

        private void AddRow(MSExcel.Worksheet sheet, int rowNumber, BcrLine line)
        {
            sheet.Cells[rowNumber, 1] = line.CostCentre.Tier1;
            sheet.Cells[rowNumber, 2] = line.CostCentre.Tier1Name;
            sheet.Cells[rowNumber, 3] = line.CostCentre.Tier2;
            sheet.Cells[rowNumber, 4] = line.CostCentre.Tier2Name;
            sheet.Cells[rowNumber, 5] = line.CostCentre.Tier3;
            sheet.Cells[rowNumber, 6] = line.CostCentre.Tier3Name;
            sheet.Cells[rowNumber, 7] = line.CostCentre.Tier4;
            sheet.Cells[rowNumber, 8] = line.CostCentre.Tier4Name;
            sheet.Cells[rowNumber, 9] = line.CostCentre.Code;
            sheet.Cells[rowNumber, 10] = line.CostCentre.CostCentreName;
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