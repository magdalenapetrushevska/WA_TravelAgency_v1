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
using WA_TravelAgency_v1.Models.Enums;

namespace WA_TravelAgency_v1.Controllers
{
    [Authorize(Roles = "Admin,Employee")]
    public class TransportController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TransportController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Transport
        public async Task<IActionResult> Index()
        {
              return _context.Transport != null ? 
                          View(await _context.Transport.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Transport'  is null.");
        }

        // GET: Transport/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Transport == null)
            {
                return NotFound();
            }

            var transport = await _context.Transport
                .FirstOrDefaultAsync(m => m.Id == id);
            if (transport == null)
            {
                return NotFound();
            }

            return View(transport);
        }

        // GET: Transport/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Transport/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Vehicle,Company,Capacity,Id")] Transport transport)
        {
            if (ModelState.IsValid)
            {
                transport.Id = Guid.NewGuid();
                _context.Add(transport);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(transport);
        }

        // GET: Transport/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Transport == null)
            {
                return NotFound();
            }

            var transport = await _context.Transport.FindAsync(id);
            if (transport == null)
            {
                return NotFound();
            }
            return View(transport);
        }

        // POST: Transport/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Vehicle,Company,Capacity,Id")] Transport transport)
        {
            if (id != transport.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(transport);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransportExists(transport.Id))
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
            return View(transport);
        }

        // GET: Transport/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Transport == null)
            {
                return NotFound();
            }

            var transport = await _context.Transport
                .FirstOrDefaultAsync(m => m.Id == id);
            if (transport == null)
            {
                return NotFound();
            }

            return View(transport);
        }

        // POST: Transport/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Transport == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Transport'  is null.");
            }
            var transport = await _context.Transport.FindAsync(id);
            if (transport != null)
            {
                _context.Transport.Remove(transport);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TransportExists(Guid id)
        {
          return (_context.Transport?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
