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

                worksheet.Cells.Clear(); // Clear the worksheet before writing

                // Header row (optional, remove if not needed)
                worksheet.Cells[1, 1].Value = "ID";
                worksheet.Cells[1, 2].Value = "First Name";
                worksheet.Cells[1, 3].Value = "Last Name";
                worksheet.Cells[1, 4].Value = "Middle Name";
                worksheet.Cells[1, 5].Value = "Category";
                worksheet.Cells[1, 6].Value = "Location Code";
                worksheet.Cells[1, 7].Value = "Position";
                worksheet.Cells[1, 12].Value = "School";

                int row = 2;
                foreach (var record in data)
                {
                    worksheet.Cells[row, 1].Value = record.Id;
                    worksheet.Cells[row, 2].Value = record.FirstName;
                    worksheet.Cells[row, 3].Value = record.LastName;
                    worksheet.Cells[row, 4].Value = record.MiddleName;
                    worksheet.Cells[row, 5].Value = record.Category;
                    worksheet.Cells[row, 6].Value = record.LocationCode;
                    worksheet.Cells[row, 7].Value = record.Position;
                    worksheet.Cells[row, 12].Value = record.School;
                    row++;
                }

                package.Save();
            }
        }
    }
}