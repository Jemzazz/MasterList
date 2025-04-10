using OfficeOpenXml;
using MasterList.Models;
using System.Collections.Generic;
using System.IO;

namespace MasterList.Services
{
    public class ExcelReaderService
    {
        public List<MList> ReadExcelFile(string filePath)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var records = new List<MList>();
            var fileInfo = new FileInfo(filePath);

            if (!fileInfo.Exists)
                throw new FileNotFoundException("Excel file not found", filePath);

            using (var package = new ExcelPackage(fileInfo))
            {
                var worksheet = package.Workbook.Worksheets[0];
                int rowCount = worksheet.Dimension.Rows;
                int colCount = worksheet.Dimension.Columns;

                // Column indexes (1-based)
                const int idCol = 1;         // A
                const int firstNameCol = 2;   // B
                const int lastNameCol = 3;    // C
                const int middleNameCol = 4;  // D
                const int categoryCol = 5;    // E
                const int locationCol = 6;    // F
                const int positionCol = 7;    // G
                const int schoolCol = 12;     // L

                int previousId = -1;

                for (int row = 2; row <= rowCount; row++)
                {
                    var idValue = worksheet.Cells[row, idCol].Text;
                    if (string.IsNullOrWhiteSpace(idValue) || idValue == "0" || idValue == "ID")
                        continue;

                    int currentId = GetIntValue(worksheet.Cells[row, idCol]);
                    if (currentId == previousId)
                        continue;

                    var firstName = GetStringValue(worksheet.Cells[row, firstNameCol]);
                    var lastName = GetStringValue(worksheet.Cells[row, lastNameCol]);

                    // Skip if essential fields are blank
                    if (string.IsNullOrWhiteSpace(firstName) && string.IsNullOrWhiteSpace(lastName))
                        continue;

                    records.Add(new MList
                    {
                        Id = GetIntValue(worksheet.Cells[row, idCol]),
                        FirstName = GetStringValue(worksheet.Cells[row, firstNameCol]),
                        LastName = GetStringValue(worksheet.Cells[row, lastNameCol]),
                        MiddleName = GetStringValue(worksheet.Cells[row, middleNameCol]),
                        Category = GetStringValue(worksheet.Cells[row, categoryCol]),
                        LocationCode = GetStringValue(worksheet.Cells[row, locationCol]),
                        Position = GetStringValue(worksheet.Cells[row, positionCol]),
                        School = GetStringValue(worksheet.Cells[row, 12]) // Column L
                    });
                }
            }

            return records;
        }

        private int GetIntValue(ExcelRange cell) => int.TryParse(cell.Text, out int result) ? result : 0;
        private string GetStringValue(ExcelRange cell) => cell.Text?.Trim() ?? string.Empty;
    }
}