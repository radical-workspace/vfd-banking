using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using BankingSystem.BLL.Interfaces;
using BankingSystem.DAL.Models;
using BankingSystem.PL.ViewModels.Admin;

public class AdminManagerController(IUnitOfWork unitOfWork, IMapper mapper) : Controller
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public IActionResult GetAllManagers()
    {
        var AdminID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var Managers = _unitOfWork.Repository<Manager>().GetAllIncluding(
                                                                    m => m.Branch,
                                                                    m => m.Tellers)
                                                                    .ToList();

        return View(Managers);
    }

    // GET: Manager/Create
    public IActionResult Create()
    {
        // Populate branches dropdown
        ViewBag.Branches = new SelectList(_unitOfWork.Repository<Branch>().GetAll(), "Id", "Name");
        return View(new ManagerVM());
    }

    // POST: Manager/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(ManagerVM model)
    {
        if (ModelState.IsValid)
        {
            try
            {
                // Create Manager entity from view model
                var manager = new Manager
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Address = model.Address,
                    SSN = model.SSN,
                    JoinDate = DateTime.Now,
                    BirthDate = model.BirthDate,
                    Salary = model.Salary,
                    BranchId = model.BranchId,
                    UserName = model.Email,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber
                };

                // Add manager to repository
                _unitOfWork.Repository<Manager>().Add(manager);

                // Save changes
                _unitOfWork.Complete();

                TempData["SuccessMessage"] = $"Manager '{manager.FirstName} {manager.LastName}' created successfully.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error creating manager: {ex.Message}");
            }
        }

        // If we got this far, something failed, redisplay form
        ViewBag.Branches = new SelectList(_unitOfWork.Repository<Branch>().GetAll(), "Id", "Name", model.BranchId);
        return View(model);
    }

    [HttpGet]
    public IActionResult Edit(string id)
    {
        var manager = _unitOfWork.Repository<Manager>().GetSingleIncluding(m => m.Id == id, m => m.Branch!, m => m.Tellers!);

        if (manager == null)
            return NotFound();

        // Get available branches
        var branches = _unitOfWork.Repository<Branch>().GetAll().ToList();

        ViewBag.Branches = branches.Select(b => new SelectListItem
        {
            Value = b.Id.ToString(),
            Text = b.Name,
            Selected = manager.Branch != null && b.Id == manager.Branch.Id
        });

        return View(manager);
    }

    [HttpPost]
    public IActionResult Edit(Manager manager, int? newBranchId)
    {
        // Fetch the existing manager from the database
        var existingManager = _unitOfWork.Repository<Manager>().GetSingleIncluding(m => m.Id == manager.Id, m => m.Branch!, m => m.Tellers!);

        if (existingManager == null)
            return NotFound();

        // Update manager properties
        existingManager.FirstName = manager.FirstName;
        existingManager.LastName = manager.LastName;
        existingManager.Address = manager.Address;
        existingManager.Salary = manager.Salary;
        existingManager.PhoneNumber = manager.PhoneNumber;

        // Only update email if changed (may require additional identity management)
        if (existingManager.Email != manager.Email)
        {
            existingManager.Email = manager.Email;
            existingManager.UserName = manager.Email;
        }

        // Get all branches for the dropdown
        var branches = _unitOfWork.Repository<Branch>().GetAll();

        // Prepare branches for view if we need to return to it
        ViewBag.Branches = branches.Select(b => new SelectListItem
        {
            Value = b.Id.ToString(),
            Text = b.Name,
            Selected = existingManager.Branch != null && b.Id == existingManager.Branch.Id
        });

        // Process branch change if a new branch is selected
        if (newBranchId.HasValue)
        {
            // Get the selected branch
            var newBranch = _unitOfWork.Repository<Branch>().GetSingleIncluding(b => b.Id == newBranchId);

            if (newBranch == null)
            {
                ModelState.AddModelError("Branch", "Selected branch does not exist.");
                return View(existingManager);
            }

            // If manager was assigned to another branch before
            if (existingManager.BranchId.HasValue && existingManager.BranchId != newBranchId)
            {
                var previousBranch = _unitOfWork.Repository<Branch>().GetSingleIncluding(b => b.Id == existingManager.BranchId);
                if (previousBranch != null && previousBranch.MyManager?.Id == existingManager.Id)
                {
                    // Unlink manager from previous branch
                    previousBranch.MyManager = null;
                    _unitOfWork.Repository<Branch>().Update(previousBranch);
                }
            }

            // Assign manager to new branch
            existingManager.BranchId = newBranchId;
            existingManager.Branch = newBranch;

            // Update branch's manager reference
            newBranch.MyManager = existingManager;
            _unitOfWork.Repository<Branch>().Update(newBranch);
        }
        else
        {
            // Manager is being unassigned from any branch
            if (existingManager.BranchId.HasValue)
            {
                var previousBranch = _unitOfWork.Repository<Branch>().GetSingleIncluding(b => b.Id == existingManager.BranchId);
                if (previousBranch != null && previousBranch.MyManager?.Id == existingManager.Id)
                {
                    // Unlink manager from previous branch
                    previousBranch.MyManager = null;
                    _unitOfWork.Repository<Branch>().Update(previousBranch);
                }
            }

            existingManager.BranchId = null;
            existingManager.Branch = null;
        }

        // Save changes
        _unitOfWork.Repository<Manager>().Update(existingManager);
        _unitOfWork.Complete();

        TempData["SuccessMessage"] = "Manager updated successfully.";
        return RedirectToAction(nameof(Details), new { id = existingManager.Id });
    }

    public IActionResult Delete(string id)
    {
        // Get the manager to be deleted
        var manager = _unitOfWork.Repository<Manager>().GetSingleIncluding(
            m => m.Id == id,
            m => m.Branch,
            m => m.Tellers
        );

        // Check if manager exists
        if (manager == null)
        {
            return NotFound();
        }

        try
        {
            // Check for related entities that might prevent deletion
            if (manager.Tellers.Any())
            {
                TempData["ErrorMessage"] = "Cannot delete manager because they are supervising tellers. Please reassign the tellers first.";
                return RedirectToAction("Index");
            }

            // Handle the branch relationship first
            if (manager.Branch != null)
            {
                // Unassign this manager from the branch
                manager.Branch.MyManager = null;
                _unitOfWork.Repository<Branch>().Update(manager.Branch);
            }

            // Now that relationships are handled, delete the manager
            _unitOfWork.Repository<Manager>().Delete(manager);
            _unitOfWork.Complete();

            TempData["SuccessMessage"] = $"Manager '{manager.FirstName} {manager.LastName}' has been successfully deleted.";
        }
        catch (Exception ex)
        {
            // Log the error
            // _logger.LogError(ex, "Error deleting manager with ID {Id}", id);

            TempData["ErrorMessage"] = $"An error occurred while trying to delete the manager: {ex.Message}";
        }

        return RedirectToAction("Index");
    }

    public IActionResult Details(string id)
    {
        var manager = _unitOfWork.Repository<Manager>().GetSingleIncluding(
            m => m.Id == id,
            m => m.Branch!,
            m => m.Tellers!
        );

        if (manager == null)
        {
            return NotFound();
        }

        return View(manager);
    }
}
