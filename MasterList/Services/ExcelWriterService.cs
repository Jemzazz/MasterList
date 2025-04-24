using OfficeOpenXml;
using MasterList.Models;
using System.Collections.Generic;
using System.IO;

namespace MasterList.Services
{
    public class ExcelWriterService
    {
        public void WriteToExcel(string filePath, List<MList> data)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var fileInfo = new FileInfo(filePath);

            using (var package = new ExcelPackage(fileInfo))
            {
                var worksheet = package.Workbook.Worksheets[0] ??
                               package.Workbook.Worksheets.Add("MasterList");

                // Find the first empty row
                int newRow = worksheet.Dimension?.Rows + 1 ?? 2;

                // Get the last record (newly added)
                var newItem = data.Last();

                // Write only the new record
                worksheet.Cells[newRow, 1].Value = newItem.Id;
                worksheet.Cells[newRow, 2].Value = newItem.FirstName;
                worksheet.Cells[newRow, 3].Value = newItem.LastName;
                worksheet.Cells[newRow, 4].Value = newItem.MiddleName;
                worksheet.Cells[newRow, 5].Value = newItem.Category;
                worksheet.Cells[newRow, 6].Value = newItem.LocationCode;
                worksheet.Cells[newRow, 7].Value = newItem.Position;
                worksheet.Cells[newRow, 12].Value = newItem.School;

                package.Save();
            }
        }
    }
}