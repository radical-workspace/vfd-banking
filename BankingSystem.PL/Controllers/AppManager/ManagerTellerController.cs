using AutoMapper;
using Azure.Core;
using BankingSystem.BLL.Interfaces;
using BankingSystem.DAL.Models;
using BankingSystem.PL.ViewModels.Auth;
using BankingSystem.PL.ViewModels.Manager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace BankingSystem.PL.Controllers.Manager
{
    [Authorize(Roles = "Manager")]
    public class ManagerTellerController(IUnitOfWork unitOfWork, IMapper mapper) : Controller
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;


        // Modify Teller To Be Inserted By Manager Instead Of Customer

        //public ActionResult CreateTeller()
        //{
        //    ViewData["FixedRole"] = "Customer";
        //    return View("~/Views/Account/Register.cshtml");
        //}


        //[HttpPost]

        //public async Task<ActionResult> CreateTeller(RegisterViewModel UserToRegister)
        //{
        //    ViewData["FixedRole"] = "Customer";
        //    var TellerHandleCustomer = _unitOfWork.Repository<Teller>().GetSingleIncluding(T => T.Id == User.FindFirst(ClaimTypes.NameIdentifier).Value);

        //    // Load roles again in case of return to the view

        //    if (UserToRegister is not null)
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            ApplicationUser appUser;
        //            Customer customer = new Customer();

        //            // Create the correct derived class based on role
        //            if (UserToRegister.Role == "Customer")
        //            {
        //                appUser = _mapper.Map<Customer>(UserToRegister);


        //                customer.FirstName = appUser.FirstName;
        //                customer.LastName = appUser.LastName;
        //                customer.UserName = appUser.UserName;
        //                customer.Email = appUser.Email;
        //                customer.SSN = appUser.SSN;
        //                customer.Address = appUser.Address;
        //                customer.BirthDate = appUser.BirthDate;
        //                customer.JoinDate = appUser.JoinDate;
        //                customer.IsDeleted = appUser.IsDeleted;
        //                customer.BranchId = TellerHandleCustomer.BranchId;







        //            }

        //            // How Cast From Applicaton User To Customer To Add BranchId

        //            else appUser = _mapper.Map<ApplicationUser>(UserToRegister);

        //            IdentityResult result = await _userManager.CreateAsync(customer, UserToRegister.Password);



        //            // Check if the user was created successfully
        //            if (result.Succeeded)
        //            {
        //                // Assign role
        //                await _userManager.AddToRoleAsync(appUser, UserToRegister.Role);

        //                // Optional: Sign in
        //                // await _signInManager.SignInAsync(appUser, false);

        //                return RedirectToAction("GetAllCustomers", new { id = User.FindFirst(ClaimTypes.NameIdentifier).Value });
        //            }
        //            else
        //            {
        //                foreach (var error in result.Errors)
        //                {
        //                    ModelState.AddModelError("", error.Description);
        //                }
        //            }
        //        }
        //    }
        //    return View("Register", UserToRegister);
        //}



        [HttpGet]
        public ActionResult AddTeller(TellerDetailsViewModel model)
        {
            var managerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var branchId = _unitOfWork.Repository<MyManager>().GetSingleIncluding(b => b.Id == managerId)?.BranchId;
            if (branchId == null) return NotFound("Branch not found");
            //return View(model);
            TempData["FixedRole"] = "Manager";
            return RedirectToAction("Register", "Account", managerId);

        }


        //// still need to update
        //[HttpPost]
        //public ActionResult AddTeller(TellerDetailsViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //    }

        //    var teller = _mapper.Map();

        //    try
        //    {
        //        _unitOfWork.Repository<Teller>().Add(teller);
        //        _unitOfWork.Complete();
        //        TempData["SuccessMessage"] = "Employee added successfully";
        //        return RedirectToAction("GetAllTellers", new { id = teller.Branch.MyManager.Id });
        //    }
        //    catch (Exception ex)
        //    {
        //        ModelState.AddModelError("", "Error saving employee: " + ex.Message);
        //        model.Branches = GetBranchSelectList();
        //        return View(model);
        //    }
        //}

        [HttpGet]
        public ActionResult GetAllTellers(string id)
        {
            var manager = _unitOfWork.Repository<MyManager>().GetSingleIncluding(b => b.Id == id);
            if (manager == null)
            {
                return NotFound($"Manager with ID {id} not found.");
            }

            var branchId = manager.BranchId;
            if (branchId == null)
            {
                return NotFound($"Branch with ID {branchId} not found.");
            }

            var employees = _unitOfWork.Repository<Teller>()
                .GetAllIncluding(e => e.Branch, e => e.Department)
                .Where(e => e.BranchId == branchId)
                .ToList();

            var tellerViewModels = _mapper.Map<List<TellerDetailsViewModel>>(employees);
            return View(tellerViewModels);
        }
        [HttpGet]
        public ActionResult GetTellerDetails(string id)
        {
            var employee = _unitOfWork.Repository<Teller>()
                .GetSingleIncluding(e => e.Id == id, e => e.Branch, e => e.Department);

            if (employee == null)
            {
                return NotFound();
            }

            var tellerDetailsViewModel = _mapper.Map<TellerDetailsViewModel>(employee);

            tellerDetailsViewModel.BranchName = employee.Branch?.Name;
            tellerDetailsViewModel.DepartmentName = employee.Department?.Name ?? "No Departments";

            return View(tellerDetailsViewModel);
        }
        [HttpGet]
        public ActionResult EditTeller(string id)
        {
            var teller = _unitOfWork.Repository<Teller>()
                .GetSingleIncluding(e => e.Id == id, e => e.Branch, e => e.Branch.MyManager, e => e.Department);
            if (teller == null)
            {
                return NotFound();
            }

            var tellerDetailsViewModel = _mapper.Map<TellerDetailsViewModel>(teller);
            tellerDetailsViewModel.BranchName = teller.Branch?.Name;
            tellerDetailsViewModel.DepartmentName = teller.Department?.Name;

            if (teller.Branch?.MyManager == null)
            {
                return NotFound("The branch does not have a manager assigned.");
            }

            return View(tellerDetailsViewModel);
        }
        [HttpPost]
        public ActionResult EditTeller(TellerDetailsViewModel tellerDetailsViewModel)
        {
            if (ModelState.IsValid)
            {
                var teller = _unitOfWork.Repository<Teller>()
                    .GetSingleIncluding(e => e.Id == tellerDetailsViewModel.Id, e => e.Branch, e => e.Department, e => e.Branch.MyManager);
                if (teller == null)
                {
                    return NotFound();
                }

                _mapper.Map(tellerDetailsViewModel, teller);
                _unitOfWork.Complete();
                return RedirectToAction("GetAllTellers", new { id = teller.Branch.MyManager.Id });
            }
            return View(tellerDetailsViewModel);
        }
        [HttpGet]
        public ActionResult DeleteTeller(string id)
        {
            var teller = _unitOfWork.Repository<Teller>()
                .GetSingleIncluding(e => e.Id == id, e => e.Branch, e => e.Branch.MyManager, e => e.Department);
            if (teller == null)
            {
                return NotFound();
            }
            var tellerDetailsViewModel = _mapper.Map<TellerDetailsViewModel>(teller);
            tellerDetailsViewModel.BranchName = teller.Branch?.Name;
            tellerDetailsViewModel.DepartmentName = teller.Department?.Name;
            return View(tellerDetailsViewModel);
        }
        [HttpPost]
        public ActionResult DeleteTeller(TellerDetailsViewModel tellerDetails)
        {
            var teller = _unitOfWork.Repository<Teller>()
                .GetSingleIncluding(e => e.Id == tellerDetails.Id, e => e.Branch, e => e.Department, e => e.Branch.MyManager);
            if (teller == null)
            {
                return NotFound();
            }
            _unitOfWork.Repository<Teller>().Delete(teller);
            _unitOfWork.Complete();
            return RedirectToAction("GetAllTellers", new { id = teller.Branch.MyManager.Id });
        }
    }
}
