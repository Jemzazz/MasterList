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
            var fileInfo = new FileInfo(filePath);
            fileInfo.Directory?.Create();

            using (var package = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Count > 0
                    ? package.Workbook.Worksheets[0]
                    : package.Workbook.Worksheets.Add("MasterList");

                // Write headers if new file
                if (worksheet.Dimension == null)
                {
                    worksheet.Cells[1, 1].Value = "ID";
                    worksheet.Cells[1, 2].Value = "First Name";
                    worksheet.Cells[1, 3].Value = "Last Name";
                    worksheet.Cells[1, 4].Value = "Middle Name";
                    worksheet.Cells[1, 5].Value = "Category";
                    worksheet.Cells[1, 6].Value = "Location Code";
                    worksheet.Cells[1, 7].Value = "Position";
                    worksheet.Cells[1, 8].Value = "School";
                }

                // Clear existing data (except header)
                if (worksheet.Dimension?.Rows > 1)
                {
                    worksheet.Cells[2, 1, worksheet.Dimension.End.Row, worksheet.Dimension.End.Column].Clear();
                }

                // Write all data
                for (int i = 0; i < data.Count; i++)
                {
                    var item = data[i];
                    int row = i + 2; // Start from row 2 (after header)

                    worksheet.Cells[row, 1].Value = item.Id;
                    worksheet.Cells[row, 2].Value = item.FirstName;
                    worksheet.Cells[row, 3].Value = item.LastName;
                    worksheet.Cells[row, 4].Value = item.MiddleName;
                    worksheet.Cells[row, 5].Value = item.Category;
                    worksheet.Cells[row, 6].Value = item.LocationCode;
                    worksheet.Cells[row, 7].Value = item.Position;
                    worksheet.Cells[row, 8].Value = item.School;
                }

                package.Save();
            }
        }
    }
}