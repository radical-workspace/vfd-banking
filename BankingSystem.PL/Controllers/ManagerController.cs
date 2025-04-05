using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.PL.Controllers
{
    public class ManagerController : Controller
    {
        public ActionResult GetAllEmployees()
        {
            return View();
        }    
    }
}
