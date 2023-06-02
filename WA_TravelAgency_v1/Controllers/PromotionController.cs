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
    public class PromotionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PromotionController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Promotion
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Promotions.Include(p => p.Offer);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Promotion/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Promotions == null)
            {
                return NotFound();
            }

            var promotion = await _context.Promotions
                .Include(p => p.Offer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (promotion == null)
            {
                return NotFound();
            }

            return View(promotion);
        }

        // GET: Promotion/Create
        public IActionResult Create()
        {
            ViewData["OfferId"] = new SelectList(_context.Offers, "Id", "Id");
            return View();
        }

        // POST: Promotion/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,OfferId,Discount,StartDateOfPromotion,EndDateOfPromotion,ImageFile,Id")] Promotion promotion)
        {
            if (ModelState.IsValid)
            {
                promotion.Id = Guid.NewGuid();
                _context.Add(promotion);

                Offer chosenOffer = _context.Offers.Find(promotion.OfferId);
                chosenOffer.PricePerPerson = chosenOffer.PricePerPerson - (chosenOffer.PricePerPerson * promotion.Discount / 100);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["OfferId"] = new SelectList(_context.Offers, "Id", "Id", promotion.OfferId);
            return View(promotion);
        }

        // GET: Promotion/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Promotions == null)
            {
                return NotFound();
            }

            var promotion = await _context.Promotions.FindAsync(id);
            if (promotion == null)
            {
                return NotFound();
            }
            ViewData["OfferId"] = new SelectList(_context.Offers, "Id", "Id", promotion.OfferId);
            return View(promotion);
        }

        // POST: Promotion/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Title,OfferId,Discount,StartDateOfPromotion,EndDateOfPromotion,Id")] Promotion promotion)
        {
            if (id != promotion.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(promotion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PromotionExists(promotion.Id))
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
            ViewData["OfferId"] = new SelectList(_context.Offers, "Id", "Id", promotion.OfferId);
            return View(promotion);
        }

        // GET: Promotion/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Promotions == null)
            {
                return NotFound();
            }

            var promotion = await _context.Promotions
                .Include(p => p.Offer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (promotion == null)
            {
                return NotFound();
            }

            return View(promotion);
        }

        // POST: Promotion/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Promotions == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Promotions'  is null.");
            }
            var promotion = await _context.Promotions.FindAsync(id);
            if (promotion != null)
            {
                _context.Promotions.Remove(promotion);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PromotionExists(Guid id)
        {
          return (_context.Promotions?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
