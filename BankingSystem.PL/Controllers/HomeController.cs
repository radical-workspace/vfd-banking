using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.PL.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
