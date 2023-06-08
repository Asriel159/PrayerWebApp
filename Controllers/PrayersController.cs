using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PrayerWebApp.Data;
using PrayerWebApp.Models;

namespace PrayerWebApp.Controllers
{
    public class PrayersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PrayersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Prayers
        public async Task<IActionResult> Index()
        {
              return _context.Prayer != null ? 
                          View(await _context.Prayer.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Prayer'  is null.");
        }

        // GET: Prayers/ShowSearchForm
        public async Task<IActionResult> ShowSearchForm()
        {
            return View();
        }

        // POST: Prayers/ShowSearchResults
        public async Task<IActionResult> ShowSearchResults(String SearchPhrase)
        {
            return View("Index", await _context.Prayer.Where(p => p.PrayerDescription.Contains(SearchPhrase)).ToListAsync());
        }

        // GET: Prayers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Prayer == null)
            {
                return NotFound();
            }

            var prayer = await _context.Prayer
                .FirstOrDefaultAsync(m => m.ID == id);
            if (prayer == null)
            {
                return NotFound();
            }

            return View(prayer);
        }
        [Authorize]

        // GET: Prayers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Prayers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,PrayerDescription,PrayerMoreDetails")] Prayer prayer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(prayer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(prayer);
        }

        // GET: Prayers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Prayer == null)
            {
                return NotFound();
            }

            var prayer = await _context.Prayer.FindAsync(id);
            if (prayer == null)
            {
                return NotFound();
            }
            return View(prayer);
        }

        // POST: Prayers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,PrayerDescription,PrayerMoreDetails")] Prayer prayer)
        {
            if (id != prayer.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(prayer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PrayerExists(prayer.ID))
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
            return View(prayer);
        }

        // GET: Prayers/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Prayer == null)
            {
                return NotFound();
            }

            var prayer = await _context.Prayer
                .FirstOrDefaultAsync(m => m.ID == id);
            if (prayer == null)
            {
                return NotFound();
            }

            return View(prayer);
        }

        // POST: Prayers/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Prayer == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Prayer'  is null.");
            }
            var prayer = await _context.Prayer.FindAsync(id);
            if (prayer != null)
            {
                _context.Prayer.Remove(prayer);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PrayerExists(int id)
        {
          return (_context.Prayer?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
