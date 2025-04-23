using BankingSystem.PL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace BankingSystem.PL.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (User.IsInRole("Admin") && User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "Admin");

            else if (User.IsInRole("Manager") && User.Identity?.IsAuthenticated == true)
                return RedirectToAction("GetAllCustomers", "ManagerCustomer", new { id = userId });

            else if (User.IsInRole("Teller") && User.Identity?.IsAuthenticated == true)
                return RedirectToAction("GetAllCustomers", "HandleCustomer", new { id = userId });

            else if (User.IsInRole("Customer") && User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Details", "CustomerProfile", new { id = userId });


            return View();
        }


        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }

    }
}
