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

                // Clear existing data except headers
                if (worksheet.Dimension?.Rows > 1)
                {
                    worksheet.Cells[2, 1, worksheet.Dimension.End.Row, worksheet.Dimension.End.Column].Clear();
                }

                // Write sorted data
                int row = 2;
                foreach (var item in data.OrderBy(x => x.Id))
                {
                    worksheet.Cells[row, 1].Value = item.Id;
                    worksheet.Cells[row, 2].Value = item.FirstName;
                    worksheet.Cells[row, 3].Value = item.LastName;
                    worksheet.Cells[row, 4].Value = item.MiddleName;
                    worksheet.Cells[row, 5].Value = item.Category;
                    worksheet.Cells[row, 6].Value = item.LocationCode;
                    worksheet.Cells[row, 7].Value = item.Position;
                    worksheet.Cells[row, 12].Value = item.School; // Column L
                }

                package.Save();
            }
        }
    }
}