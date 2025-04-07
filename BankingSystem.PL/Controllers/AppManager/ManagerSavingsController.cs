using AutoMapper;
using BankingSystem.BLL.Interfaces;
using BankingSystem.DAL.Models;
using BankingSystem.PL.ViewModels.Manager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BankingSystem.PL.Controllers.AppManager
{

    [Authorize(Roles = "Manager")]
    public class ManagerSavingsController(IUnitOfWork unitOfWork, IMapper mapper) : Controller
    {
        readonly private IUnitOfWork _unitOfWork = unitOfWork;
        readonly private IMapper _mapper = mapper;

        public ActionResult GetAllSavings(string id) // managerId
        {
            if (string.IsNullOrEmpty(id)) return RedirectToAction("Index");

            var manager = _unitOfWork.Repository<MyManager>().GetSingleIncluding(b => b.Id == id);

            if (manager?.BranchId == null) return NotFound("Manager or branch not found");


            var savings = _unitOfWork.Repository<Savings>()
                .GetAll()
                .Where(s => s.BranchId == manager.BranchId)
                .ToList();

            var viewModel = _mapper.Map<List<SavingsViewModel>>(savings);

            // Store manager ID for form posts
            ViewBag.ManagerId = id;
            TempData["ManagerId"] = id; // For post-redirect-get pattern

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateSaving(SavingsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Return to view with errors
                TempData["Error"] = "Invalid data";
                return RedirectToAction("GetAllSavings", new { id = TempData["ManagerId"] });
            }

            var saving = _unitOfWork.Repository<Savings>().Get(model.Id);
            if (saving == null)
            {
                return NotFound();
            }

            // Selective update
            saving.Currency = model.Currency;
            saving.Balance = model.Balance;

            try
            {
                _unitOfWork.Complete();
                TempData["Success"] = "Update successful";
            }
            catch (Exception)
            {
                TempData["Error"] = "Update failed";
            }

            // Preserve manager ID across redirect
            return RedirectToAction("GetAllSavings", new { id = TempData["ManagerId"] });
        }

        //[HttpPost]
        //// still need to update
        //public ActionResult AddTeller(TellerDetailsViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        model.Branches = GetBranchSelectList();
        //        return View(model);
        //    }

        //    var teller = new Teller
        //    {
        //        FirstName = model.Name.Trim(),
        //        Email = model.Email.Trim().ToLower(),
        //        PhoneNumber = model.PhoneNumber.Trim(),
        //        BranchId = model.BranchID
        //    };

        //    try
        //    {
        //        _unitOfWork.Repository<Teller>().Add(teller);
        //        _unitOfWork.Complete();
        //        TempData["SuccessMessage"] = "Employee added successfully";
        //        return RedirectToAction("GetAllTellers");
        //    }
        //    catch (Exception ex)
        //    {
        //        ModelState.AddModelError("", "Error saving employee: " + ex.Message);
        //        model.Branches = GetBranchSelectList();
        //        return View(model);
        //    }
        //}
    }
}
