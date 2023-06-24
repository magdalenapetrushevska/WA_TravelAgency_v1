using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using WA_TravelAgency_v1.Data;
using WA_TravelAgency_v1.Models.DomainModels;
using Constants = WA_TravelAgency_v1.Models.Constants.Constants;
using WA_TravelAgency_v1.Models.Enums;
using Stripe;
using Stripe.TestHelpers;
using Microsoft.AspNetCore.Identity;
using WA_TravelAgency_v1.Models.Identity;
using Newtonsoft.Json;
using System.Text;
using GemBox.Document;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using ExcelDataReader;
using DocumentFormat.OpenXml.Bibliography;
using MimeKit;
using MailKit.Net.Smtp;

namespace WA_TravelAgency_v1.Controllers
{
    [Authorize]
    public class ReservationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> userManager;

        public ReservationController(ApplicationDbContext context)
        {
            _context = context;
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");
        }

        // GET: Reservation
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Reservation.Include(r => r.Offer);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Reservation/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Reservation == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservation
                .Include(r => r.Offer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // GET: Reservation/Create
        public IActionResult Create()
        {
            ViewData["OfferId"] = new SelectList(_context.Offers, "Id", "Id");
            return View();
        }

        // POST: Reservation/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OfferId,ReservedBy,NumOfPassengers,Id")] Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var loggedInUser = _context.Users.Find(loggedInUserId);
                reservation.UserId = loggedInUserId;
                reservation.ReservationDate = DateTime.Now;
                reservation.Passenger = loggedInUser;
                reservation.ReservedBy = reservation.ReservedBy;

                Offer chosenOffer = _context.Offers.Find(reservation.OfferId);
                if(chosenOffer.МinNumOfPassForGratis <= reservation.NumOfPassengers)
                {
                    var numOfGratis = reservation.NumOfPassengers % chosenOffer.МinNumOfPassForGratis;
                    reservation.AmountToPay = chosenOffer.PricePerPerson * (reservation.NumOfPassengers - numOfGratis);
                    reservation.NumOfGratis = numOfGratis;
                }
                else
                {
                    reservation.AmountToPay = chosenOffer.PricePerPerson * reservation.NumOfPassengers;
                    reservation.NumOfGratis = 0;
                }
                reservation.AmountPaid = Constants.initialPaidAmount;
                reservation.Paid = NoYes.No;
                reservation.Status = OfferStatus.Active;

                reservation.Id = Guid.NewGuid();
                _context.Add(reservation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["OfferId"] = new SelectList(_context.Offers, "Id", "Id", reservation.OfferId);
            return View(reservation);
        }

        // GET: Reservation/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Reservation == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservation.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }
            ViewData["OfferId"] = new SelectList(_context.Offers, "Id", "Id", reservation.OfferId);
            return View(reservation);
        }

        // POST: Reservation/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("OfferId,AmountToPay,ReservationDate,Paid,AmountPaid,Status,Id")] Reservation reservation)
        {
            if (id != reservation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reservation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationExists(reservation.Id))
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
            ViewData["OfferId"] = new SelectList(_context.Offers, "Id", "Id", reservation.OfferId);
            return View(reservation);
        }

        // GET: Reservation/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Reservation == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservation
                .Include(r => r.Offer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // POST: Reservation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Reservation == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Reservation'  is null.");
            }
            var reservation = await _context.Reservation.FindAsync(id);
            if (reservation != null)
            {
                _context.Reservation.Remove(reservation);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservationExists(Guid id)
        {
          return (_context.Reservation?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        [Authorize]
        public async Task<IActionResult> MyReservations()
        {
            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var applicationDbContext = _context.Reservation
                .Include(r => r.Offer)
                .Include(r => r.Passenger)
                .Where(m => m.UserId == loggedInUserId);
            return View(await applicationDbContext.ToListAsync());
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PayOrder(string stripeEmail, string stripeToken, Guid id)
        {
            var customerService = new Stripe.CustomerService();
            var chargeService = new ChargeService();
            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var loggedInUser = _context.Users.Find(loggedInUserId);
            Reservation payReservation = _context.Reservation.Find(id);

            var customer = customerService.Create(new CustomerCreateOptions
            {
                Email = stripeEmail,
                Source = stripeToken
            });

            var charge = chargeService.Create(new ChargeCreateOptions
            {
                Amount = (Convert.ToInt32(payReservation.AmountToPay) * 100),
                Description = "Payment for" + payReservation.OfferId,
                Currency = "usd",
                Customer = customer.Id
            });

            if (charge.Status == "succeeded")
            {
                payReservation.AmountPaid += payReservation.AmountToPay;
                payReservation.Paid = NoYes.Yes;
                _context.Reservation.Update(payReservation);
                await _context.SaveChangesAsync();

                var email = new MimeMessage();
                var body = "This email serves as a confirmation that payment has been done.\n "+"Offer:  "+ payReservation.OfferId.ToString() +"\n Amount paid: "+ payReservation.AmountToPay.ToString();
                email.From.Add(MailboxAddress.Parse("tatyana.von22@ethereal.email"));
                email.To.Add(MailboxAddress.Parse("tatyana.von22@ethereal.email"));

                email.Subject = "Payment Confirmation";
                email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = body };

                using var smtp = new SmtpClient();
                smtp.Connect("smtp.ethereal.email", 587, MailKit.Security.SecureSocketOptions.StartTls);
                smtp.Authenticate("tatyana.von22@ethereal.email", "g34nXAWsR5Yfu18up1");
                smtp.Send(email);
                smtp.Disconnect(true);



                if (User.IsInRole("User"))
                {
                    return RedirectToAction("MyReservations", "Reservation");
                }
                else
                {
                    return RedirectToAction("Index", "Reservation");
                }

            }

            if (User.IsInRole("User"))
            {
                return RedirectToAction("MyReservations", "Reservation");
            }
            else
            {
                return RedirectToAction("Index", "Reservation");
            }
        }

        public FileContentResult CreateInvoice(Guid id)
        {
            var result = _context.Reservation.Find(id);

            var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Invoice.docx");
            var document = DocumentModel.Load(templatePath);

            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var loggedInUser = _context.Users.Find(loggedInUserId);


            document.Content.Replace("{{ReservationNumber}}", result.Id.ToString());
            document.Content.Replace("{{UserName}}", loggedInUser.Name);

            StringBuilder sb = new StringBuilder();

            sb.Append("Offer id: " + result.OfferId + "\n");
            sb.Append("Reservation date: " + result.ReservationDate + "\n");
            sb.Append("Reservation status: " + result.Status + "\n");
            sb.Append("Number of passengers: " + result.NumOfPassengers + "\n");
            sb.Append("Number of gratis: " + result.NumOfGratis);

            document.Content.Replace("{{ReservationDetails}}", sb.ToString());
            document.Content.Replace("{{TotalPrice}}", result.AmountToPay.ToString() + "$");

            var stream = new MemoryStream();

            document.Save(stream, new PdfSaveOptions());

            return File(stream.ToArray(), new PdfSaveOptions().ContentType, "ExportInvoice.pdf");
        }


        [HttpGet]
        public FileContentResult ExportAllReservations()
        {
            string fileName = "Reservations.xlsx";
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add("All reservations");

                worksheet.Cell(1, 1).Value = "Reservation Id";
                worksheet.Cell(1, 2).Value = "Customer Email";
                worksheet.Cell(1, 3).Value = "Offer";
                worksheet.Cell(1, 4).Value = "Amount to pay";
                worksheet.Cell(1, 5).Value = "Paid";
                worksheet.Cell(1, 6).Value = "Amount paid";
                worksheet.Cell(1, 7).Value = "Status";
                worksheet.Cell(1, 8).Value = "Number of passengers";

                List<Reservation> reservations = new List<Reservation>();
                reservations = _context.Reservation.Include(r => r.Offer).Include(r => r.Passenger).ToList();


                for (int i = 1; i <= reservations.Count; i++)
                {
                    var item = reservations[i - 1];

                    worksheet.Cell(i + 1, 1).Value = item.Id.ToString();
                    worksheet.Cell(i + 1, 2).Value = item.Passenger != null ? item.Passenger.Email.ToString() : "";
                    worksheet.Cell(i + 1, 3).Value = item.Offer != null ? item.Offer.Name.ToString() : "";
                    worksheet.Cell(i + 1, 4).Value = item.AmountToPay.ToString();
                    worksheet.Cell(i + 1, 5).Value = item.Paid.ToString();
                    worksheet.Cell(i + 1, 6).Value = item.AmountPaid.ToString();
                    worksheet.Cell(i + 1, 7).Value = item.Status.ToString();
                    worksheet.Cell(i + 1, 8).Value = item.NumOfPassengers.ToString();

                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(content, contentType, fileName);
                }

            }
        }

        public IActionResult ImportReservations(IFormFile file)
        {

            //make a copy
            string pathToUpload = $"{Directory.GetCurrentDirectory()}\\files\\{file.FileName}";

            using (FileStream fileStream = System.IO.File.Create(pathToUpload))
            {
                file.CopyTo(fileStream);

                fileStream.Flush();
            }

            this.getAllReservationsFromFileAsync(file.FileName);

            return RedirectToAction("Index", "Reservation");
        }

        private async Task getAllReservationsFromFileAsync(string fileName)
        {
            string filePath = $"{Directory.GetCurrentDirectory()}\\files\\{fileName}";

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);


            using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    while (reader.Read())
                    {
                        Reservation newReservation = new Reservation();
                        newReservation.Id = Guid.NewGuid();
                        newReservation.OfferId = new Guid(reader.GetValue(0).ToString());
                        newReservation.AmountToPay = Convert.ToInt32(reader.GetValue(1));
                        newReservation.ReservationDate = (DateTime)reader.GetValue(2);
                        newReservation.Paid = Enum.Parse<NoYes>(reader.GetValue(3).ToString());
                        newReservation.AmountPaid = Convert.ToInt32(reader.GetValue(4));
                        newReservation.Status = Enum.Parse<OfferStatus>(reader.GetValue(5).ToString());
                        newReservation.UserId = (string)reader.GetValue(6);
                        newReservation.NumOfPassengers = Convert.ToInt32(reader.GetValue(7));
                        newReservation.NumOfGratis = Convert.ToInt32(reader.GetValue(8));

                        _context.Add(newReservation);
                    }
                    await _context.SaveChangesAsync();
                }
            }
        }


        // CreateMyReservation
        public IActionResult CreateMyReservation(Guid? id)
        {
            ViewData["OfferId"] = new SelectList(_context.Offers, "Id", "Id", id);
            return View();
        }
    }
}
