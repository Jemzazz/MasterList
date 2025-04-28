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

            // Delete existing file if it exists
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                var worksheet = package.Workbook.Worksheets.Add("MasterList");

                // Create header row
                worksheet.Cells[1, 1].Value = "#";
                worksheet.Cells[1, 2].Value = "First Name";
                worksheet.Cells[1, 3].Value = "Last Name";
                worksheet.Cells[1, 4].Value = "Middle Name";
                worksheet.Cells[1, 5].Value = "Category";
                worksheet.Cells[1, 6].Value = "Location Code";
                worksheet.Cells[1, 7].Value = "Position";
                worksheet.Cells[1, 8].Value = "School";

                // Style the header
                using (var range = worksheet.Cells[1, 1, 1, 8])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                }

                // Write data with sequential numbers
                for (int i = 0; i < data.Count; i++)
                {
                    var row = i + 2; // Start from row 2 (1-based index)
                    var record = data[i];

                    worksheet.Cells[row, 1].Value = i + 1; // Sequential number
                    worksheet.Cells[row, 2].Value = record.FirstName;
                    worksheet.Cells[row, 3].Value = record.LastName;
                    worksheet.Cells[row, 4].Value = record.MiddleName;
                    worksheet.Cells[row, 5].Value = record.Category;
                    worksheet.Cells[row, 6].Value = record.LocationCode;
                    worksheet.Cells[row, 7].Value = record.Position;
                    worksheet.Cells[row, 8].Value = string.IsNullOrWhiteSpace(record.School) ? "Not specified" : record.School;

                    // Center the sequence number
                    worksheet.Cells[row, 1].Style.HorizontalAlignment =
                        OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                }

                // Auto-fit all columns
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                // Add filters
                worksheet.Cells[1, 1, 1, 8].AutoFilter = true;

                // Freeze header row
                worksheet.View.FreezePanes(2, 1);

                package.Save();
            }
        }
    }
}