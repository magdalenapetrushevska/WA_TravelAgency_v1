using Microsoft.AspNetCore.Mvc;

namespace WA_TravelAgency_v1.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
