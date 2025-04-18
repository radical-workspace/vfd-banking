using BankingSystem.BLL.Interfaces;
using BankingSystem.DAL.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BankingSystem.PL.Controllers.AppAdmin
{
    //[Authorize(Roles = "Admin")]
    public class AdminManagerController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public AdminManagerController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }
        // GET: AdminManagerController
        public ActionResult GetAllManagers()
        {
            var Managers = _unitOfWork.Repository<Manager>().GetAllIncluding(M => M.Branch).ToList();

            return View(Managers);
        }

        // GET: AdminManagerController/Details/5
        public ActionResult GetManagerDetails(string id)
        {
            var ManagerToDisplay = _unitOfWork.Repository<Manager>().GetAllIncluding(M => M.Branch).Where(M =>M.Id == id).FirstOrDefault();
            return View(ManagerToDisplay);
        }

        // GET: AdminManagerController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminManagerController/Create
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

        // GET: AdminManagerController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AdminManagerController/Edit/5
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

        // GET: AdminManagerController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AdminManagerController/Delete/5
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
