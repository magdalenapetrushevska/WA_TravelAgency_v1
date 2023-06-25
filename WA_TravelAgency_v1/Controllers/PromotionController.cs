using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using WA_TravelAgency_v1.Data;
using WA_TravelAgency_v1.Models.DomainModels;

namespace WA_TravelAgency_v1.Controllers
{
    public class PromotionController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public PromotionController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            this._hostEnvironment = hostEnvironment;
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
        public IActionResult Create(Guid? id)
        {
            ViewData["OfferId"] = new SelectList(_context.Offers, "Id", "Name", id);
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

                //Save image to wwwroot/image
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(promotion.ImageFile.FileName);
                string extension = Path.GetExtension(promotion.ImageFile.FileName);
                promotion.ImageName = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath + "/Images/PromotionImages", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await promotion.ImageFile.CopyToAsync(fileStream);
                }

                _context.Add(promotion);

                Offer chosenOffer = _context.Offers.Find(promotion.OfferId);
                chosenOffer.PricePerPerson = chosenOffer.PricePerPerson - (chosenOffer.PricePerPerson * promotion.Discount / 100);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["OfferId"] = new SelectList(_context.Offers, "Id", "Name", promotion.OfferId);
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
            ViewData["OfferId"] = new SelectList(_context.Offers, "Id", "Name", promotion.OfferId);
            return View(promotion);
        }

        // POST: Promotion/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Discount,StartDateOfPromotion,EndDateOfPromotion,Id")] Promotion promotion)
        {
            if (id != promotion.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //Save image to wwwroot/image
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = Path.GetFileNameWithoutExtension(promotion.ImageFile.FileName);
                    string extension = Path.GetExtension(promotion.ImageFile.FileName);
                    promotion.ImageName = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    string path = Path.Combine(wwwRootPath + "/Images/PromotionImages", fileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await promotion.ImageFile.CopyToAsync(fileStream);
                    }

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
            ViewData["OfferId"] = new SelectList(_context.Offers, "Id", "Name", promotion.OfferId);
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
            Offer chosenOffer = await _context.Offers.FindAsync(promotion.OfferId);
            chosenOffer.PromotionId = null;
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
