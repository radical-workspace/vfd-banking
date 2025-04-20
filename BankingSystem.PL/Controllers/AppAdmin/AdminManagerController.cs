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
        return View();
    }

    // POST: Manager/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(ManagerVM model)
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
                PhoneNumber = model.PhoneNumber,
                PasswordHash = "Test@123"
            };

            // Add manager to repository
            _unitOfWork.Repository<Manager>().Add(manager);

            // Save changes
            _unitOfWork.Complete();
            TempData["SuccessMessage"] = $"Manager '{manager.FirstName} {manager.LastName}' created successfully.";
            ViewBag.Branches = new SelectList(_unitOfWork.Repository<Branch>().GetAll(), "Id", "Name", model.BranchId);

            return RedirectToAction("Index", "Admin");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, $"Error creating manager: {ex.Message}");
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


        ViewBag.Branches = new SelectList(branches, "Id", "Name", manager.BranchId);

        ManagerVM managerVM = new ManagerVM
        {
            //Id = manager.Id,
            FirstName = manager.FirstName,
            LastName = manager.LastName,
            Address = manager.Address,
            SSN = manager.SSN,
            //JoinDate = manager.JoinDate,
            BirthDate = manager.BirthDate,
            Salary = manager.Salary,
            PhoneNumber = manager.PhoneNumber,
            Email = manager.Email,
            BranchId = manager.BranchId
        };

        return View(managerVM);
    }

    [HttpPost]
    public IActionResult Edit(ManagerVM managerVM)
    {
        // Fetch the existing manager from the database
        var existingManager = _unitOfWork.Repository<Manager>().GetSingleIncluding(m => m.Id == managerVM.Id, m => m.Branch!, m => m.Tellers!);

        if (existingManager == null)
            return NotFound();

        // Update manager properties
        existingManager.FirstName = managerVM.FirstName;
        existingManager.LastName = managerVM.LastName;
        existingManager.Address = managerVM.Address;
        existingManager.Salary = managerVM.Salary;
        existingManager.PhoneNumber = managerVM.PhoneNumber;

        // Only update email if changed (may require additional identity management)
        if (existingManager.Email != managerVM.Email)
        {
            existingManager.Email = managerVM.Email;
            existingManager.UserName = managerVM.Email;
        }

        // Get all branches for the dropdown
        var branches = _unitOfWork.Repository<Branch>().GetAll();

        ViewBag.Branches = new SelectList(branches, "Id", "Name", managerVM.BranchId);


        // Process branch change if a new branch is selected
        if (managerVM.BranchId.HasValue)
        {
            // Get the selected branch
            var newBranch = _unitOfWork.Repository<Branch>().GetSingleIncluding(b => b.Id == managerVM.BranchId);

            if (newBranch == null)
            {
                ModelState.AddModelError("Branch", "Selected branch does not exist.");
                return View(existingManager);
            }

            // If manager was assigned to another branch before
            if (existingManager.BranchId.HasValue && existingManager.BranchId != managerVM.BranchId)
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
            existingManager.BranchId = managerVM.BranchId;
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
        return RedirectToAction(nameof(Index), nameof(Admin));
    }


    [HttpPost]
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

        return RedirectToAction("Index", "Admin");
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