using BankingSystem.BLL;
using BankingSystem.BLL.Interfaces;
using BankingSystem.DAL.Models;
using BankingSystem.PL.ViewModels.Manager;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.PL.Controllers.AppManager
{
    public class ManagerBranchController(IUnitOfWork unitOfWork) : Controller
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public ActionResult GetBranchDetails(string id)
        {
            var Manager = _unitOfWork.Repository<MyManager>().GetSingleIncluding(b => b.Id == id);

            var Branches = _unitOfWork.Repository<Branch>().GetSingleIncluding(
                                                                b => b.Id == Manager.BranchId,
                                                                l => l.Loans,
                                                                c => c.Customers,
                                                                t => t.Tellers,
                                                                d => d.Departments,
                                                                s => s.Savings,
                                                                );

            if (Branches == null) return NotFound();

            var BranchDetails = new BranchDetailsViewModel
            {
                Id = Branches.Id,
                Name = Branches.Name,
                Location = Branches.Location,
                Opens = Branches.Opens,
                Closes = Branches.Closes,
                Loans = Branches.Loans,
                Customers = Branches.Customers,
                Tellers = Branches.Tellers,
                Departments = Branches.Departments,
                Savings = Branches.Savings,
            };
            return View(BranchDetails);
        }
        public ActionResult Create()
        {
            return View();
        }
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
        public ActionResult Edit(int id)
        {
            return View();
        }
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
        public ActionResult Delete(int id)
        {
            return View();
        }
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
