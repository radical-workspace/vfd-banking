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
    public class AdminBankBranchController(IUnitOfWork unitOfWork, IMapper mapper) : Controller
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;


        public IActionResult GetBankDetails(string id)
        {
            var Bank = _unitOfWork.Repository<Bank>().GetAllIncluding(
                        b => b.Admin,
                        b => b.Branches
                    ).FirstOrDefault();


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
            return View(new BranchVM());
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
                    return RedirectToAction("Index");
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
            var availableManagers = _unitOfWork.Repository<Manager>().GetAll().Where(m => m.BranchId == null).ToList();

            ViewBag.Managers = availableManagers.Select(m => new SelectListItem
            {
                Value = m.Id.ToString(),
                Text = m.FirstName,
                Selected = branch.MyManager != null && m.Id == branch.MyManager.Id
            });

            return View(branch);
        }
        [HttpPost]
        public IActionResult Edit(Branch branch, string newManagerId)
        {
            // Fetch the existing branch from the database
            var existingBranch = _unitOfWork.Repository<Branch>().GetSingleIncluding(b => b.Id == branch.Id, b => b.MyManager!);

            if (existingBranch == null)
                return NotFound();

            // Update branch properties
            existingBranch.Name = branch.Name;
            existingBranch.Location = branch.Location;
            existingBranch.Opens = branch.Opens;
            existingBranch.Closes = branch.Closes;

            // Get all unassigned managers for the dropdown
            var availableManagers = _unitOfWork.Repository<Manager>().GetAll().Where(m => m.BranchId == null);

            // Prepare managers for view if we need to return to it
            ViewBag.Managers = availableManagers.Select(m => new SelectListItem
            {
                Value = m.Id,
                Text = $"{m.FirstName} {m.LastName}",
                Selected = existingBranch.MyManager != null && m.Id == existingBranch.MyManager.Id
            });

            // Process manager change if a new manager is selected
            if (newManagerId != "")
            {
                // Get the selected manager
                var newManager = _unitOfWork.Repository<Manager>().GetSingleIncluding(m => m.Id == newManagerId);

                if (newManager == null)
                {
                    ModelState.AddModelError("Manager", "Selected manager does not exist.");
                    return View(existingBranch);
                }

                // Update the manager reference
                existingBranch.MyManager.Id = newManagerId;

                // If there was a previous manager, unassign the branch from them
                if (existingBranch.MyManager != null && existingBranch.MyManager.Id != newManagerId)
                {
                    var previousManager = _unitOfWork.Repository<Manager>().GetSingleIncluding(m => m.Id == existingBranch.MyManager.Id);
                    if (previousManager != null)
                    {
                        previousManager.BranchId = null;
                        _unitOfWork.Repository<Manager>().Update(previousManager);
                    }
                }

                // Assign branch to new manager
                newManager.BranchId = existingBranch.Id;
                _unitOfWork.Repository<Manager>().Update(newManager);
            }

            // Save changes
            _unitOfWork.Repository<Branch>().Update(existingBranch);
            _unitOfWork.Complete();

            TempData["SuccessMessage"] = "Branch updated successfully.";
            return RedirectToAction(nameof(Details), new { id = existingBranch.Id });
        }

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
                    return RedirectToAction("Index");
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

            return RedirectToAction("Index");
        }
    }
}
