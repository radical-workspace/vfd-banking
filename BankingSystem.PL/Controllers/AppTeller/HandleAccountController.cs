using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.PL.Controllers.AppTeller
{
    public class HandleAccountController : Controller
    {
        // GET: HandleAccountController
        public ActionResult Index()
        {
            return View();
        }

        // GET: HandleAccountController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: HandleAccountController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HandleAccountController/Create
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

        // GET: HandleAccountController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: HandleAccountController/Edit/5
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

        // GET: HandleAccountController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: HandleAccountController/Delete/5
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
