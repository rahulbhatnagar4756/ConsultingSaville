using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.IO;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;


namespace Library.Tools.Conversions.Excel
{
    public static class ExcelConvert
    {
              
        /// <summary>
        /// Converts a DataTable to an Excel file and returns the file as a byte array.
        /// </summary>
        /// <param name="dataTable">The DataTable to convert.</param>
        /// <returns>Byte array representing the Excel file.</returns>
        public static byte[]? ToExcel(this DataTable dataTable)
        {
            if (dataTable == null) return null;
            string SheetName = "Sheet1";
            SheetName = dataTable.TableName != null ? dataTable.TableName : SheetName;

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add(dataTable, SheetName);

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }
        

    }
}
