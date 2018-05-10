using System;
using System.Data;
using MSExcel = Microsoft.Office.Interop.Excel;

namespace Unit4
{
    internal class Excel
    {
        public void WriteToExcel(string path, DataSet data)
        {
            var excelApp = new MSExcel.Application();
            var excelWorkBook = excelApp.Workbooks.Add();

            foreach (DataTable table in data.Tables)
            {
                if (table.TableName.Equals("parameters"))
                {
                    continue;
                }

                //Add a new worksheet to workbook with the Datatable name
                MSExcel.Worksheet excelWorkSheet = excelWorkBook.Sheets.Add() as MSExcel.Worksheet;
                excelWorkSheet.Name = Guid.NewGuid().ToString("N").Substring(0, 16);

                for (int i = 1; i < table.Columns.Count + 1; i++)
                {
                    excelWorkSheet.Cells[1, i] = table.Columns[i - 1].ColumnName;
                }

                for (int j = 0; j < table.Rows.Count; j++)
                {
                    for (int k = 0; k < table.Columns.Count; k++)
                    {
                        excelWorkSheet.Cells[j + 2, k + 1] = table.Rows[j].ItemArray[k].ToString();
                    }
                }
            }

            excelWorkBook.SaveAs(path);
            excelWorkBook.Close();
            excelApp.Quit();
        }
    }
}