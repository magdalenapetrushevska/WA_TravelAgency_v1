using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WA_TravelAgency_v1.Data;
using WA_TravelAgency_v1.Models.DomainModels;

namespace WA_TravelAgency_v1.Controllers
{
    public class OfferParametersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OfferParametersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: OfferParameters
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.OfferParameters.Include(o => o.Offer);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: OfferParameters/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.OfferParameters == null)
            {
                return NotFound();
            }

            var offerParameters = await _context.OfferParameters
                .Include(o => o.Offer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (offerParameters == null)
            {
                return NotFound();
            }

            return View(offerParameters);
        }

        // GET: OfferParameters/Create
        public IActionResult Create()
        {
            ViewData["OfferId"] = new SelectList(_context.Offers, "Id", "Id");
            return View();
        }

        // POST: OfferParameters/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("МinNumOfPassengersForSureRealization,МinNumOfPassForGratis,OfferId,Id")] OfferParameters offerParameters)
        {
            if (ModelState.IsValid)
            {
                offerParameters.Id = Guid.NewGuid();
                _context.Add(offerParameters);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["OfferId"] = new SelectList(_context.Offers, "Id", "Id", offerParameters.OfferId);
            return View(offerParameters);
        }

        // GET: OfferParameters/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.OfferParameters == null)
            {
                return NotFound();
            }

            var offerParameters = await _context.OfferParameters.FindAsync(id);
            if (offerParameters == null)
            {
                return NotFound();
            }
            ViewData["OfferId"] = new SelectList(_context.Offers, "Id", "Id", offerParameters.OfferId);
            return View(offerParameters);
        }

        // POST: OfferParameters/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("МinNumOfPassengersForSureRealization,МinNumOfPassForGratis,OfferId,Id")] OfferParameters offerParameters)
        {
            if (id != offerParameters.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(offerParameters);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OfferParametersExists(offerParameters.Id))
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
            ViewData["OfferId"] = new SelectList(_context.Offers, "Id", "Id", offerParameters.OfferId);
            return View(offerParameters);
        }

        // GET: OfferParameters/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.OfferParameters == null)
            {
                return NotFound();
            }

            var offerParameters = await _context.OfferParameters
                .Include(o => o.Offer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (offerParameters == null)
            {
                return NotFound();
            }

            return View(offerParameters);
        }

        // POST: OfferParameters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.OfferParameters == null)
            {
                return Problem("Entity set 'ApplicationDbContext.OfferParameters'  is null.");
            }
            var offerParameters = await _context.OfferParameters.FindAsync(id);
            if (offerParameters != null)
            {
                _context.OfferParameters.Remove(offerParameters);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OfferParametersExists(Guid id)
        {
          return (_context.OfferParameters?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
