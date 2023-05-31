using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WA_TravelAgency_v1.Data;
using WA_TravelAgency_v1.Models.DomainModels;

namespace WA_TravelAgency_v1.Controllers
{
    [Authorize(Roles = "Admin,Employee")]
    public class DestinationController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DestinationController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Destination
        public async Task<IActionResult> Index()
        {
              return _context.Destinatons != null ? 
                          View(await _context.Destinatons.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Destinatons'  is null.");
        }

        // GET: Destination/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Destinatons == null)
            {
                return NotFound();
            }

            var destination = await _context.Destinatons
                .FirstOrDefaultAsync(m => m.Id == id);
            if (destination == null)
            {
                return NotFound();
            }

            return View(destination);
        }

        // GET: Destination/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Destination/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Country,City,Accommodation,Id")] Destination destination)
        {
            if (ModelState.IsValid)
            {
                destination.Id = Guid.NewGuid();
                _context.Add(destination);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(destination);
        }

        // GET: Destination/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Destinatons == null)
            {
                return NotFound();
            }

            var destination = await _context.Destinatons.FindAsync(id);
            if (destination == null)
            {
                return NotFound();
            }
            return View(destination);
        }

        // POST: Destination/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Country,City,Accommodation,Id")] Destination destination)
        {
            if (id != destination.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(destination);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DestinationExists(destination.Id))
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
            return View(destination);
        }

        // GET: Destination/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Destinatons == null)
            {
                return NotFound();
            }

            var destination = await _context.Destinatons
                .FirstOrDefaultAsync(m => m.Id == id);
            if (destination == null)
            {
                return NotFound();
            }

            return View(destination);
        }

        // POST: Destination/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Destinatons == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Destinatons'  is null.");
            }
            var destination = await _context.Destinatons.FindAsync(id);
            if (destination != null)
            {
                _context.Destinatons.Remove(destination);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DestinationExists(Guid id)
        {
          return (_context.Destinatons?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
