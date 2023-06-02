using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using MimeKit;
using Stripe;
using WA_TravelAgency_v1.Data;
using WA_TravelAgency_v1.Models.DomainModels;
using WA_TravelAgency_v1.Models.Enums;
using WA_TravelAgency_v1.Models.Identity;
using Constants = WA_TravelAgency_v1.Models.Constants.Constants;

namespace WA_TravelAgency_v1.Controllers
{
    public class OfferController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public OfferController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            this._hostEnvironment = hostEnvironment;
        }

        // GET: Offer
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Offers.Include(o => o.Destination).Include(o => o.Transport);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Offer/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Offers == null)
            {
                return NotFound();
            }

            var offer = await _context.Offers
                .Include(o => o.Destination)
                .Include(o => o.Transport)
                .Include(o => o.Reservations)
                .Include(o => o.Transport)
                .Include(o => o.Destination)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (offer == null)
            {
                return NotFound();
            }

            return View(offer);
        }

        // GET: Offer/Create
        public IActionResult Create()
        {
            ViewData["DestinationId"] = new SelectList(_context.Destinatons, "Id", "Country");
            ViewData["TransportId"] = new SelectList(_context.Transport, "Id", "Id");
            return View();
        }

        // POST: Offer/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,TransportId,DestinationId,Type,PricePerPerson,FromDate,ToDate,Status,ImageFile,Id")] Offer offer)
        {
            Destination chosenDestinaion = _context.Destinatons.Find(offer.DestinationId);
            offer.Destination = chosenDestinaion;
            Transport chosenTransport = _context.Transport.Find(offer.TransportId);
            offer.Transport = chosenTransport;
            if (ModelState.IsValid)
            {
                offer.Id = Guid.NewGuid();
                offer.OriginalPrice = offer.PricePerPerson;

                //Save image to wwwroot/image
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(offer.ImageFile.FileName);
                string extension = Path.GetExtension(offer.ImageFile.FileName);
                offer.ImageName = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath + "/Images/OfferImages", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await offer.ImageFile.CopyToAsync(fileStream);
                }

                _context.Add(offer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DestinationId"] = new SelectList(_context.Destinatons, "Id", "Id", offer.DestinationId);
            ViewData["TransportId"] = new SelectList(_context.Transport, "Id", "Id", offer.TransportId);
            return View(offer);
        }

        // GET: Offer/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Offers == null)
            {
                return NotFound();
            }

            var offer = await _context.Offers.FindAsync(id);
            if (offer == null)
            {
                return NotFound();
            }
            ViewData["DestinationId"] = new SelectList(_context.Destinatons, "Id", "Id", offer.DestinationId);
            ViewData["TransportId"] = new SelectList(_context.Transport, "Id", "Id", offer.TransportId);
            return View(offer);
        }

        // POST: Offer/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Name,TransportId,DestinationId,Type,PricePerPerson,OriginalPrice,FromDate,ToDate,Status,ImageFile,Id")] Offer offer)
        {
            if (id != offer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //Save image to wwwroot/image
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = Path.GetFileNameWithoutExtension(offer.ImageFile.FileName);
                    string extension = Path.GetExtension(offer.ImageFile.FileName);
                    offer.ImageName = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    string path = Path.Combine(wwwRootPath + "/Images/OfferImages", fileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await offer.ImageFile.CopyToAsync(fileStream);
                    }

                    _context.Update(offer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OfferExists(offer.Id))
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
            ViewData["DestinationId"] = new SelectList(_context.Destinatons, "Id", "Id", offer.DestinationId);
            ViewData["TransportId"] = new SelectList(_context.Transport, "Id", "Id", offer.TransportId);
            return View(offer);
        }

        // GET: Offer/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Offers == null)
            {
                return NotFound();
            }

            var offer = await _context.Offers
                .Include(o => o.Destination)
                .Include(o => o.Transport)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (offer == null)
            {
                return NotFound();
            }

            return View(offer);
        }

        // POST: Offer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Offers == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Offers'  is null.");
            }
            var offer = await _context.Offers.FindAsync(id);
            if (offer != null)
            {
                _context.Offers.Remove(offer);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OfferExists(Guid id)
        {
          return (_context.Offers?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        [Authorize]
        public async Task<IActionResult> MakeAReservation(Guid id)
        {
            Reservation newReservation = new Reservation();

            newReservation.Id = Guid.NewGuid();
            newReservation.OfferId = id;

            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var loggedInUser = _context.Users.Find(loggedInUserId);
            newReservation.UserId = loggedInUserId;
            newReservation.Passenger = loggedInUser;
            newReservation.ReservationDate = DateTime.Now;

            Offer chosenOffer = _context.Offers.Find(newReservation.OfferId);
            newReservation.AmountToPay = chosenOffer.PricePerPerson;
            newReservation.AmountPaid = Constants.initialPaidAmount;
            newReservation.Paid = NoYes.No;
            newReservation.Status = OfferStatus.Active;

            _context.Add(newReservation);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index","Reservation");
        }

        public async Task<IActionResult> SendMail()
        {
            var email = new MimeMessage();
            var body = "Example message for test email. Check whather it will work for sending email reservation test for offer.";
            email.From.Add(MailboxAddress.Parse("jeramie.satterfield@ethereal.email"));
            email.To.Add(MailboxAddress.Parse("jeramie.satterfield@ethereal.email"));

            email.Subject = "Send email in Asp.net core web app";
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = body };

            using var smtp = new SmtpClient();
            smtp.Connect("smtp.ethereal.email", 587, MailKit.Security.SecureSocketOptions.StartTls);
            smtp.Authenticate("jeramie.satterfield@ethereal.email", "Es9gN3cavCfPddFHxe");
            smtp.Send(email);
            smtp.Disconnect(true);

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SearchOffer(string id)
        {
            if (id!=null || id!="")
            {
                ViewBag.SearchWord = id;
                List<Offer> list = new();
                list = _context.Offers.Where(m => m.Name.Contains(id)).ToList();

                return View(list);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> SummerOffers()
        {
            var applicationDbContext = _context.Offers.Include(o => o.Destination).Include(o => o.Transport).Where(m => m.Type == OfferType.Summer);
            return View(await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> WinterOffers()
        {
            var applicationDbContext = _context.Offers.Include(o => o.Destination).Include(o => o.Transport).Where(m => m.Type == OfferType.Winter);
            return View(await applicationDbContext.ToListAsync());
        }

    }
}
