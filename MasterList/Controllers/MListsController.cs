using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MasterList.Data;
using MasterList.Models;

namespace MasterList.Controllers
{
    public class MListsController : Controller
    {
        private readonly MasterListContext _context;

        public MListsController(MasterListContext context)
        {
            _context = context;
        }

        // GET: MLists
        public async Task<IActionResult> Index()
        {
            return View(await _context.MList.ToListAsync());
        }

        // GET: MLists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mList = await _context.MList
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mList == null)
            {
                return NotFound();
            }

            return View(mList);
        }

        // GET: MLists/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MLists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,MiddleName,Category,LocationCode,Position,School")] MList mList)
        {
            if (ModelState.IsValid)
            {
                _context.Add(mList);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(mList);
        }

        // GET: MLists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mList = await _context.MList.FindAsync(id);
            if (mList == null)
            {
                return NotFound();
            }
            return View(mList);
        }

        // POST: MLists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,MiddleName,Category,LocationCode,Position,School")] MList mList)
        {
            if (id != mList.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mList);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MListExists(mList.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(mList);
        }

        // GET: MLists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mList = await _context.MList
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mList == null)
            {
                return NotFound();
            }

            return View(mList);
        }

        // POST: MLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mList = await _context.MList.FindAsync(id);
            if (mList != null)
            {
                _context.MList.Remove(mList);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MListExists(int id)
        {
            return _context.MList.Any(e => e.Id == id);
        }
    }
}
