using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MasterList.Models;
using MasterList.Services;
using System.IO;

namespace MasterList.Controllers
{
    public class MListsController : Controller
    {
        private readonly ExcelReaderService _reader;
        private readonly ExcelWriterService _writer;
        private readonly string _excelPath;

        public MListsController(ExcelReaderService reader, ExcelWriterService writer)
        {
            _reader = reader;
            _writer = writer;
            _excelPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data", "201 FILES MASTERLIST.xlsx");

            // Initialize if file doesn't exist
            if (!System.IO.File.Exists(_excelPath))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(_excelPath));
                _writer.WriteToExcel(_excelPath, new List<MList>());
            }
        }

        // GET: MLists
        public IActionResult Index()
        {
            // Debugging: Check if file exists
            var data = _reader.ReadExcelFile(_excelPath);

            // Filter out any records with ID=0 (header row)
            data = data.Where(x => x.Id != 0).ToList();

            return View(data);
        }

        // GET: MLists/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null) return NotFound();

            try
            {
                var data = _reader.ReadExcelFile(_excelPath);
                var item = data.FirstOrDefault(m => m.Id == id);
                return item == null ? NotFound() : View(item);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error loading details: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: MLists/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MLists/Create
        private static readonly object _fileLock = new object();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(MList mList)
        {
            if (!ModelState.IsValid)
                return View(mList);

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data", "201 FILES MASTERLIST.xlsx");

            var reader = new ExcelReaderService();
            List<MList> existingRecords = reader.ReadExcelFile(filePath);

            // Generate new unique ID
            int maxId = existingRecords.Any() ? existingRecords.Max(x => x.Id) : 200;
            mList.Id = maxId + 1;

            // Add the new record
            existingRecords.Add(mList);

            // Save to Excel
            var writer = new ExcelWriterService();
            writer.WriteToExcel(filePath, existingRecords);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult FixDuplicates()
        {
            lock (_fileLock)
            {
                var data = _reader.ReadExcelFile(_excelPath);

                // Remove duplicates (keep first occurrence)
                var cleanData = data
                    .GroupBy(x => x.Id)
                    .Select(g => g.First())
                    .OrderBy(x => x.Id)
                    .ToList();

                // Reassign sequential IDs
                for (int i = 0; i < cleanData.Count; i++)
                {
                    cleanData[i].Id = i + 1;
                }

                _writer.WriteToExcel(_excelPath, cleanData);
                TempData["SuccessMessage"] = $"Fixed {data.Count - cleanData.Count} duplicates";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: MLists/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null) return NotFound();

            try
            {
                var data = _reader.ReadExcelFile(_excelPath);
                var item = data.FirstOrDefault(m => m.Id == id);
                return item == null ? NotFound() : View(item);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error loading record for edit: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: MLists/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, MList mList)
        {
            if (id != mList.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                var data = _reader.ReadExcelFile(_excelPath);
                var index = data.FindIndex(m => m.Id == id);
                if (index == -1)
                    return NotFound();

                data[index] = mList;
                _writer.WriteToExcel(_excelPath, data);

                TempData["SuccessMessage"] = "Record updated successfully";
                return RedirectToAction(nameof(Details), new { id = mList.Id });
            }
            return View(mList);
        }

        // GET: MLists/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();

            try
            {
                var data = _reader.ReadExcelFile(_excelPath);
                var item = data.FirstOrDefault(m => m.Id == id);
                return item == null ? NotFound() : View(item);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error loading record for deletion: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: MLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var data = _reader.ReadExcelFile(_excelPath);
            var item = data.FirstOrDefault(m => m.Id == id);
            if (item == null)
                return NotFound();

            data.Remove(item);
            _writer.WriteToExcel(_excelPath, data);

            TempData["SuccessMessage"] = "Record deleted successfully";
            return RedirectToAction(nameof(Index));
        }
    }
}