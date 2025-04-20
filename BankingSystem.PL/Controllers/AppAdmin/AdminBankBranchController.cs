using AutoMapper;
using BankingSystem.BLL.Interfaces;
using BankingSystem.DAL.Models;
using BankingSystem.PL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using System.Security.Claims;

namespace BankingSystem.PL.Controllers.AppAdmin
{
    public class AdminBankBranchController(IUnitOfWork unitOfWork) : Controller
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;


        public IActionResult GetBankDetails(string id)
        {
            var Bank = _unitOfWork.Repository<Bank>().GetAllIncluding(
                        b => b.Admin,
                        b => b.Branches
                    ).FirstOrDefault();

            ViewBag.accountsNumber = _unitOfWork.Repository<Account>().GetAll().Count();
            ViewBag.tellersNumber = _unitOfWork.Repository<Teller>().GetAll().Count();
            ViewBag.customersNumber = _unitOfWork.Repository<Customer>().GetAll().Count();
            return View(Bank);
        }

        public IActionResult GetAllBranches()
        {
            var AdminID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var Branches = _unitOfWork.Repository<Branch>().GetAllIncluding(
                                                                            b => b.MyManager,
                                                                            b => b.Loans,
                                                                            b => b.Customers,
                                                                            b => b.Tellers,
                                                                            b => b.Departments,
                                                                            b => b.Savings,
                                                                            b => b.Reservations).ToList();
            return View(Branches);
        }


        // GET: Branch/Create
        public IActionResult Create()
        {
            // Populate banks dropdown
            ViewBag.Banks = new SelectList(_unitOfWork.Repository<Bank>().GetAll(), "Id", "Name");
            return View();
        }

        // POST: Branch/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(BranchVM model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Create Branch entity from view model
                    var branch = new Branch
                    {
                        Name = model.Name,
                        Location = model.Location,
                        CreatedAt = DateTime.Now,
                        Opens = model.Opens,
                        Closes = model.Closes
                    };

                    // Add branch to repository
                    _unitOfWork.Repository<Branch>().Add(branch);

                    // Create default savings account for the branch
                    var branchSavings = new Savings
                    {
                        BranchId = branch.Id,
                        Currency = "EGP",
                        Branch = branch,
                        Balance = 1000000, // Initial branch funds
                    };

                    _unitOfWork.Repository<Savings>().Add(branchSavings);

                    // Save changes
                    _unitOfWork.Complete();

                    TempData["SuccessMessage"] = $"Branch '{branch.Name}' created successfully.";
                    return RedirectToAction("Index", "Admin");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"Error creating branch: {ex.Message}");
                }
            }

            return View(model);
        }


        [HttpGet]
        public IActionResult Edit(int id)
        {
            var branch = _unitOfWork.Repository<Branch>().GetSingleIncluding(b => b.Id == id, b => b.MyManager!);

            if (branch == null)
                return NotFound();

            // Get available managers (not assigned to any branch)
            var availableManagers = _unitOfWork.Repository<Manager>().GetAll()
                .Where(m => m.BranchId == null)
                .ToList();

            // Create proper SelectList
            ViewBag.Managers = new SelectList(
                availableManagers,
                nameof(Manager.Id),
                nameof(Manager.FullName), // Add a FullName property to Manager
                branch.MyManager?.Id
            );

            return View(branch);
        }


        [HttpPost]
        public IActionResult Edit(BranchVM model)
        {
            var existingBranch = _unitOfWork.Repository<Branch>().GetSingleIncluding(
                b => b.Id == model.Id,
                b => b.MyManager!
            );

            if (existingBranch == null)
                return NotFound();

            // Always repopulate managers dropdown first
            ViewBag.Managers = new SelectList(
                _unitOfWork.Repository<Manager>().GetAll().Where(m => m.BranchId == null),
                nameof(Manager.Id),
                nameof(Manager.FullName),
                model.ManagerId  // Correct selected value
            );

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                // Update branch properties
                existingBranch.Name = model.Name;
                existingBranch.Location = model.Location;
                existingBranch.Opens = model.Opens;
                existingBranch.Closes = model.Closes;

                // Handle manager assignment
                var currentManagerId = existingBranch.MyManager?.Id;

                if (model.ManagerId != currentManagerId)
                {
                    // Remove current manager if exists
                    if (currentManagerId != null)
                    {
                        var previousManager = _unitOfWork.Repository<Manager>().GetAll().Where(b=>b.Id== currentManagerId).FirstOrDefault();
                        if (previousManager != null)
                        {
                            previousManager.BranchId = null;
                            _unitOfWork.Repository<Manager>().Update(previousManager);
                        }
                        existingBranch.MyManager = null;
                    }

                    // Assign new manager if selected
                    if (!string.IsNullOrEmpty(model.ManagerId))
                    {
                        var newManager = _unitOfWork.Repository<Manager>().GetAll().Where(b => b.Id == model.ManagerId).FirstOrDefault();
                        if (newManager == null)
                        {
                            ModelState.AddModelError("ManagerId", "Selected manager does not exist");
                            return View(model);
                        }

                        newManager.BranchId = existingBranch.Id;
                        existingBranch.MyManager = newManager;
                        _unitOfWork.Repository<Manager>().Update(newManager);
                    }
                }

                _unitOfWork.Repository<Branch>().Update(existingBranch);
                _unitOfWork.Complete();

                TempData["SuccessMessage"] = "Branch updated successfully";
                return RedirectToAction(nameof(Details), new { id = existingBranch.Id });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while saving: " + ex.Message);
                return View(model);
            }
        }


        [HttpPost]
        public IActionResult Delete(int id)
        {
            // Get the branch to be deleted
            var branch = _unitOfWork.Repository<Branch>().GetSingleIncluding(
                b => b.Id == id,
                b => b.MyManager,
                b => b.Tellers,
                b => b.Departments,
                b => b.Customers,
                b => b.Loans,
                b => b.Savings,
                b => b.Reservations
            );

            // Check if branch exists
            if (branch == null)
            {
                return NotFound();
            }

            try
            {
                // Check for related entities that might prevent deletion
                if (branch.Customers.Any() || branch.Loans.Any() || branch.Savings.Any())
                {
                    TempData["ErrorMessage"] = "Cannot delete branch because it has associated customers, loans, savings accounts.";
                    return RedirectToAction("Index", "Admin");
                }

                // Handle the branch manager relationship first
                if (branch.MyManager != null)
                {
                    // Unassign this branch from the manager
                    branch.MyManager.BranchId = null;
                    _unitOfWork.Repository<Manager>().Update(branch.MyManager);
                }

                // Reassign or remove tellers
                foreach (var teller in branch.Tellers)
                {
                    // Either delete tellers or set BranchId to null
                    teller.BranchId = null;
                    _unitOfWork.Repository<Teller>().Update(teller);
                }

                // Now that relationships are handled, delete the branch
                _unitOfWork.Repository<Branch>().Delete(branch);
                _unitOfWork.Complete();

                TempData["SuccessMessage"] = $"Branch '{branch.Name}' has been successfully deleted.";
            }
            catch (Exception ex)
            {
                // Log the error
                // _logger.LogError(ex, "Error deleting branch with ID {Id}", id);

                TempData["ErrorMessage"] = $"An error occurred while trying to delete the branch: {ex.Message}";
            }

            return RedirectToAction("Index", "Admin");
        }
    }
}
