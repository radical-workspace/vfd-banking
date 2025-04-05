using Azure.Core;
using BankingSystem.BLL.Interfaces;
using BankingSystem.DAL.Models;
using BankingSystem.PL.ViewModels.Manager;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Data;
using System.Reflection;
using System;

namespace BankingSystem.PL.Controllers.Manager
{
    [Authorize(Roles = "Manager")]
    public class ManagerTellerController(IUnitOfWork unitOfWork) : Controller
    {
        readonly private IUnitOfWork _unitOfWork = unitOfWork;

        public ActionResult GetAllTellers()
        {
            var employees = _unitOfWork.Repository<Teller>().GetAllIncluding(e => e.Branch);
            List<TellerDetailsViewModel> tellerDetailsViewModels = new ();

            foreach (var employee in employees)
            {
                var tellerDetailsViewModel = new TellerDetailsViewModel
                {
                    Id = employee.Id,
                    Name = employee.FirstName,
                    Email = employee.Email!,
                    PhoneNumber = employee.PhoneNumber!,
                    BranchName = employee.Branch?.Name ?? "No Branch Info",
                    //Salary = employee.Salary ?? 0
                };
                tellerDetailsViewModels.Add(tellerDetailsViewModel);
            }
            return View(tellerDetailsViewModels);
        }
        public ActionResult GetTellerDetails(string id)
        {
            var employee = _unitOfWork.Repository<Teller>().GetSingleIncluding(e => e.Id == id, e => e.Branch);
            if (employee == null)
            {
                return NotFound();
            }
            var tellerDetailsViewModel = new TellerDetailsViewModel
            {
                Id = employee.Id,
                Name = employee.FirstName,
                Email = employee.Email,
                PhoneNumber = employee.PhoneNumber,
                BranchName = employee.Branch?.Name ?? "No Branch Info",
            };
            return View(tellerDetailsViewModel);
        }
        [HttpGet]
        // still need to update
        public ActionResult AddTeller()
        {
            var viewModel = new
            {
                Branches = GetBranchSelectList()
            };
            
            //ViewData ---> only persists for the current request.When redirecting to another action, ViewData is lost.
            //TempData ---> persists data across redirects, making it suitable for passing the fixed role.
            TempData["FixedRole"] = "Teller";
            return RedirectToAction("Register", "Account");
        }
        [HttpPost]
        // still need to update
        public ActionResult AddTeller(TellerDetailsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Branches = GetBranchSelectList();
                return View(model);
            }

            var teller = new Teller
            {
                FirstName = model.Name.Trim(),
                Email = model.Email.Trim().ToLower(),
                PhoneNumber = model.PhoneNumber.Trim(),
                BranchId = model.BranchID
            };

            try
            {
                _unitOfWork.Repository<Teller>().Add(teller);
                _unitOfWork.Complete();
                TempData["SuccessMessage"] = "Employee added successfully";
                return RedirectToAction("GetAllTellers");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error saving employee: " + ex.Message);
                model.Branches = GetBranchSelectList();
                return View(model);
            }
        }
        private List<SelectListItem> GetBranchSelectList()
        {
            return _unitOfWork.Repository<Branch>()
                .GetAll()
                .Select(b => new SelectListItem
                {
                    Value = b.Id.ToString(),
                    Text = b.Name
                })
                .ToList();
        }
        [HttpGet]
        public ActionResult EditTeller(string id)
        {
            var employee = _unitOfWork.Repository<Teller>().GetSingleIncluding(e => e.Id == id, e => e.Branch);
            if (employee == null)
            {
                return NotFound();
            }
            var tellerDetailsViewModel = new TellerDetailsViewModel
            {
                Id = employee.Id,
                Name = employee.FirstName,
                Email = employee.Email,
                PhoneNumber = employee.PhoneNumber,
                BranchName = employee.Branch?.Name ?? "No Branch Info",
            };
            return View( tellerDetailsViewModel);
        }
        [HttpPost]
        public ActionResult EditTeller(TellerDetailsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var teller = _unitOfWork.Repository<Teller>().GetSingleIncluding(e => e.Id == model.Id, e => e.Branch);
            if (teller == null)
            {
                return NotFound();
            }

            teller.FirstName = model.Name;
            teller.Email = model.Email;
            teller.PhoneNumber = model.PhoneNumber;

            _unitOfWork.Repository<Teller>().Update(teller);
            _unitOfWork.Complete();

            return RedirectToAction("GetAllTellers");
        }
        [HttpGet]
        public ActionResult DeleteTeller(string id)
        {
            var employee = _unitOfWork.Repository<Teller>().GetSingleIncluding(e => e.Id == id, e => e.Branch);
            if (employee == null)
            {
                return NotFound();
            }
            var tellerDetailsViewModel = new TellerDetailsViewModel
            {
                Id = employee.Id,
                Name = employee.FirstName,
                Email = employee.Email,
                PhoneNumber = employee.PhoneNumber,
                BranchName = employee.Branch?.Name ?? "No Branch Info",
            };
            return View( tellerDetailsViewModel);
        }
        [HttpPost]
        public ActionResult DeleteTeller(TellerDetailsViewModel tellerDetails)
        {
            var teller = _unitOfWork.Repository<Teller>().GetSingleIncluding(e => e.Id == tellerDetails.Id, e => e.Branch);
            if (teller == null)
            {
                return NotFound();
            }
            _unitOfWork.Repository<Teller>().Delete(teller);
            _unitOfWork.Complete();
            return RedirectToAction("GetAllTellers");
        }
    }
}
