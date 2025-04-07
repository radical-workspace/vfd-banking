using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.PL.Controllers.AppTeller
{
    public class HandleCustomerController : Controller
    {
        // GET: HandleCustomerController
        public ActionResult Index()
        {
            return View();
        }

        // GET: HandleCustomerController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: HandleCustomerController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HandleCustomerController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: HandleCustomerController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: HandleCustomerController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: HandleCustomerController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: HandleCustomerController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
