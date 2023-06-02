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
    public class OfferPromotionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OfferPromotionController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: OfferPromotion
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.OfferPromotions.Include(o => o.Offer);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: OfferPromotion/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.OfferPromotions == null)
            {
                return NotFound();
            }

            var offerPromotion = await _context.OfferPromotions
                .Include(o => o.Offer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (offerPromotion == null)
            {
                return NotFound();
            }

            return View(offerPromotion);
        }

        // GET: OfferPromotion/Create
        public IActionResult Create()
        {
            ViewData["OfferId"] = new SelectList(_context.Offers, "Id", "Id");
            return View();
        }

        // POST: OfferPromotion/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,OfferId,Discount,StartDateOfPromotion,EndDateOfPromotion,Id")] OfferPromotion offerPromotion)
        {
            if (ModelState.IsValid)
            {
                offerPromotion.Id = Guid.NewGuid();
                _context.Add(offerPromotion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["OfferId"] = new SelectList(_context.Offers, "Id", "Id", offerPromotion.OfferId);
            return View(offerPromotion);
        }

        // GET: OfferPromotion/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.OfferPromotions == null)
            {
                return NotFound();
            }

            var offerPromotion = await _context.OfferPromotions.FindAsync(id);
            if (offerPromotion == null)
            {
                return NotFound();
            }
            ViewData["OfferId"] = new SelectList(_context.Offers, "Id", "Id", offerPromotion.OfferId);
            return View(offerPromotion);
        }

        // POST: OfferPromotion/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Title,OfferId,Discount,StartDateOfPromotion,EndDateOfPromotion,Id")] OfferPromotion offerPromotion)
        {
            if (id != offerPromotion.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(offerPromotion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OfferPromotionExists(offerPromotion.Id))
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
            ViewData["OfferId"] = new SelectList(_context.Offers, "Id", "Id", offerPromotion.OfferId);
            return View(offerPromotion);
        }

        // GET: OfferPromotion/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.OfferPromotions == null)
            {
                return NotFound();
            }

            var offerPromotion = await _context.OfferPromotions
                .Include(o => o.Offer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (offerPromotion == null)
            {
                return NotFound();
            }

            return View(offerPromotion);
        }

        // POST: OfferPromotion/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.OfferPromotions == null)
            {
                return Problem("Entity set 'ApplicationDbContext.OfferPromotions'  is null.");
            }
            var offerPromotion = await _context.OfferPromotions.FindAsync(id);
            if (offerPromotion != null)
            {
                _context.OfferPromotions.Remove(offerPromotion);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OfferPromotionExists(Guid id)
        {
          return (_context.OfferPromotions?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
