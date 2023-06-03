using DocumentFormat.OpenXml.Spreadsheet;
using ExcelDataReader;
using GemBox.Document;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Stripe.Terminal;
using System.Collections.Generic;
using System.Text;
using WA_TravelAgency_v1.Data;
using WA_TravelAgency_v1.Models.DomainModels;
using WA_TravelAgency_v1.Models.Enums;
using WA_TravelAgency_v1.Models.Identity;

namespace WA_TravelAgency_v1.Controllers
{
    [Authorize(Roles = "Admin")]
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public EmployeeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ImportEmployees(IFormFile file)
        {

            //make a copy
            string pathToUpload = $"{Directory.GetCurrentDirectory()}\\files\\{file.FileName}";

            using (FileStream fileStream = System.IO.File.Create(pathToUpload))
            {
                file.CopyTo(fileStream);

                fileStream.Flush();
            }

            this.getAllEmployeesFromFileAsync(file.FileName);

            return RedirectToAction("Index", "Home");
        }

        private async Task getAllEmployeesFromFileAsync(string fileName)
        {
            string filePath = $"{Directory.GetCurrentDirectory()}\\files\\{fileName}";

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);


            using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    while (reader.Read())
                    {
                        var userCheck = _userManager.FindByEmailAsync(reader.GetValue(0).ToString()).Result;

                        if (userCheck == null)
                        {
                            var user = new ApplicationUser
                            {
                                UserName = reader.GetValue(0).ToString(),
                                NormalizedUserName = reader.GetValue(0).ToString(),
                                Email = reader.GetValue(0).ToString(),
                                EmailConfirmed = true,
                                PhoneNumberConfirmed = true,
                                Name = reader.GetValue(1).ToString(),
                            };

                            var result = _userManager.CreateAsync(user, reader.GetValue(2).ToString()).Result;

                            if (result.Succeeded)
                            {
                                await _userManager.AddToRoleAsync(user, Roles.Employee.ToString());
                            }
                        }
                    }

                }
            }
        }

    }
}
