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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(MList mList)
        {
            if (ModelState.IsValid)
            {
                var data = _reader.ReadExcelFile(_excelPath);
                mList.Id = data.Any() ? data.Max(x => x.Id) + 1 : 1;
                data.Add(mList);
                _writer.WriteToExcel(_excelPath, data);
                return RedirectToAction(nameof(Index));
            }
            return View(mList);
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
            if (id != mList.Id) return NotFound();

            if (!ModelState.IsValid) return View(mList);

            try
            {
                var data = _reader.ReadExcelFile(_excelPath);
                var index = data.FindIndex(m => m.Id == id);
                if (index == -1) return NotFound();

                data[index] = mList;
                _writer.WriteToExcel(_excelPath, data);
                TempData["SuccessMessage"] = "Record updated successfully";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error updating record: " + ex.Message);
                return View(mList);
            }
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
            try
            {
                var data = _reader.ReadExcelFile(_excelPath);
                var item = data.FirstOrDefault(m => m.Id == id);
                if (item == null) return NotFound();

                data.Remove(item);
                _writer.WriteToExcel(_excelPath, data);
                TempData["SuccessMessage"] = "Record deleted successfully";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error deleting record: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }
    }
}