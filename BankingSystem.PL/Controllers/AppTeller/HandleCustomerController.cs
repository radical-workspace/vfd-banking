using AutoMapper;
using BankingSystem.BLL.Interfaces;
using BankingSystem.BLL.Repositories;
using BankingSystem.BLL.Services;
using BankingSystem.DAL.Models;
using BankingSystem.PL.ViewModels.Auth;
using BankingSystem.PL.ViewModels.Teller;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Operations;
using System.Globalization;
using System.Security.Claims;

namespace BankingSystem.PL.Controllers.AppTeller
{
    [Authorize(Roles = "Teller")]
    public class HandleCustomerController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        //
        private readonly IGenericRepository<Account> _genericRepository;
        private readonly ISearchPaginationRepo<Customer> _searchPaginationRepo;

        public HandleCustomerController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, IMapper mapper, 
            IGenericRepository<Account> genericRepository, ISearchPaginationRepo<Customer> searchPaginationRepo)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _mapper = mapper;

            //
            _genericRepository = genericRepository;
            _searchPaginationRepo = searchPaginationRepo;
        }


        public ActionResult GetAllCustomers(string id, string? filter)
        {
            var TellerHandleCustomer = _unitOfWork.Repository<Teller>().GetSingleIncluding(T => T.Id == id);
            //var TellerFromTellerTabe= 

            var branchId = TellerHandleCustomer?.BranchId;


            var Customers = _unitOfWork.Repository<MyCustomer>()
                .GetAllIncluding(C => C.Branch)
                .Where(C => C.BranchId == branchId)
                .ToList();


            if (filter != null)
            {
                var monthNumber = DateTime.ParseExact(filter, "MMMM", CultureInfo.InvariantCulture).Month;
                Customers = Customers.Where(c => c.JoinDate.Month == monthNumber).ToList();
            }

            var cutomerstoView = _mapper.Map<List<Customer>, List<CustomersViewModel>>(Customers);
            ViewBag.TotalRecords = Customers.Count();

            return View(cutomerstoView);
        }


        public ActionResult GetCustomerDetails(string id)
        {
            var Customer = _unitOfWork.Repository<MyCustomer>()
                .GetSingleIncluding(C => C.Id == id, C => C.Branch, C => C.Loans, C => C.Transactions, C => C.Cards, C => C.SupportTickets, C => C.Accounts);

            var mappedCustomer = _mapper.Map<MyCustomer, CustomerDetailsViewModel>(Customer);


            return View("GetCustomerDetails", mappedCustomer);
        }


        public ActionResult CreateCustomer()
        {
            ViewData["FixedRole"] = "Customer";
            return View("~/Views/Account/Register.cshtml");
        }


        [HttpPost]

        // public async Task<ActionResult> CreateCustomer(RegisterViewModel UserToRegister)
        // {
        //     ViewData["FixedRole"] = "Customer";
        //     var TellerHandleCustomer = _unitOfWork.Repository<Teller>().GetSingleIncluding(T => T.Id == User.FindFirst(ClaimTypes.NameIdentifier).Value);
        //
        //     // Load roles again in case of return to the view
        //
        //     if (UserToRegister is not null)
        //     {
        //         if (ModelState.IsValid)
        //         {
        //             ApplicationUser appUser;
        //             Customer customer = new Customer();
        //
        //             // Create the correct derived class based on role
        //             if (UserToRegister.Role == "Customer")
        //             {
        //                 appUser = _mapper.Map<Customer>(UserToRegister);
        //
        //
        //                 customer.FirstName = appUser.FirstName;
        //                 customer.LastName = appUser.LastName;
        //                 customer.UserName = appUser.UserName;
        //                 customer.Email = appUser.Email;
        //                 customer.SSN = appUser.SSN;
        //                 customer.Address = appUser.Address;
        //                 customer.BirthDate = appUser.BirthDate;
        //                 customer.JoinDate = appUser.JoinDate;
        //                 customer.IsDeleted = appUser.IsDeleted;
        //                 customer.BranchId = TellerHandleCustomer.BranchId;
        //
        //
        //
        //             }
        //
        //             // How Cast From Applicaton User To Customer To Add BranchId
        //
        //             else appUser = _mapper.Map<ApplicationUser>(UserToRegister);
        //
        //             IdentityResult result = await _userManager.CreateAsync(customer, UserToRegister.Password);
        //
        //
        //
        //             // Check if the user was created successfully
        //             if (result.Succeeded)
        //             {
        //                 // Assign role
        //                 await _userManager.AddToRoleAsync(appUser, UserToRegister.Role);
        //
        //                 // Optional: Sign in
        //                 // await _signInManager.SignInAsync(appUser, false);
        //
        //                 return RedirectToAction("GetAllCustomers", new { id = User.FindFirst(ClaimTypes.NameIdentifier).Value });
        //             }
        //             else
        //             {
        //                 foreach (var error in result.Errors)
        //                 {
        //                     ModelState.AddModelError("", error.Description);
        //                 }
        //             }
        //         }
        //     }
        //     return View("Register",UserToRegister);
        // }
        public async Task<ActionResult> CreateCustomer(RegisterViewModel UserToRegister)
        {
            ViewData["FixedRole"] = "Customer";

            var tellerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var TellerHandleCustomer = _unitOfWork.Repository<Teller>().GetSingleIncluding(t => t.Id == tellerId);

            if (UserToRegister is not null)
            {
                if (ModelState.IsValid)
                {
                    IdentityResult result;
                    ApplicationUser appUser;

                    if (UserToRegister.Role == "Customer")
                    {
                        // Map directly to Customer
                        var customer = _mapper.Map<MyCustomer>(UserToRegister);

                        // Manually assign the branch from teller
                        customer.BranchId = TellerHandleCustomer.BranchId;

                        // Save to database
                        result = await _userManager.CreateAsync(customer, UserToRegister.Password);
                        appUser = customer;
                    }
                    else
                    {
                        // For other roles, map to ApplicationUser
                        appUser = _mapper.Map<ApplicationUser>(UserToRegister);
                        result = await _userManager.CreateAsync(appUser, UserToRegister.Password);
                    }

                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(appUser, UserToRegister.Role);
                        return RedirectToAction("GetAllCustomers", new { id = tellerId });
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }
            }

            return View("Register", UserToRegister);
        }



        public ActionResult EditCustomer(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var customer = _userManager.Users.OfType<MyCustomer>().FirstOrDefault(c => c.Id == id);
            if (customer == null) return NotFound();

            var model = new EditCustomerViewModel
            {
                Id = customer.Id,
                
                Email = customer.Email,
                SSN = customer.SSN,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Address = customer.Address,
                BirthDate = customer.BirthDate,
                JoinDate = customer.JoinDate,
               
            };

            return View(model);
        }


        // POST: HandleCustomerController/Edit/5
        [HttpPost]
     
        public async Task<ActionResult> EditCustomer(string id, EditCustomerViewModel model)
        {
            if (id != model.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var customer = _userManager.Users.OfType<MyCustomer>().FirstOrDefault(c => c.Id == id);
                if (customer == null) return NotFound();

              
                customer.Email = model.Email;
                customer.SSN = model.SSN;
                customer.FirstName = model.FirstName;
                customer.LastName = model.LastName;
                customer.Address = model.Address;
                customer.BirthDate = model.BirthDate;
                customer.JoinDate = model.JoinDate;
             

                var result = await _userManager.UpdateAsync(customer);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(GetAllCustomers), new { id = User.FindFirst(ClaimTypes.NameIdentifier).Value });
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }



        public ActionResult DeleteCustomer(string id)
        {
            var Customer = _unitOfWork.Repository<MyCustomer>()
                 .GetSingleIncluding(C => C.Id == id, C => C.Branch, C => C.Loans, C => C.Transactions, C => C.Cards, C => C.SupportTickets, C => C.Accounts);

            var mappedCustomerToDeleted = _mapper.Map<MyCustomer, CustomerDetailsViewModel>(Customer);


            return View(mappedCustomerToDeleted);
        }


        [HttpPost]
        public ActionResult DeleteCustomer(CustomerDetailsViewModel customerDetailsViewModel)
        {

            if (customerDetailsViewModel is not null)
            {
                var customerToBeDeleted = _unitOfWork.Repository<MyCustomer>()
                  .GetSingleIncluding(C => C.Id == customerDetailsViewModel.Id,
                  C => C.Branch, C => C.Loans, C => C.Transactions, C => C.Cards,
                  C => C.SupportTickets, C => C.Accounts);

                _unitOfWork.Repository<MyCustomer>().Delete(customerToBeDeleted);
                _unitOfWork.Complete();
                return RedirectToAction(nameof(GetAllCustomers), new { id = User.FindFirst(ClaimTypes.NameIdentifier).Value });


            }
            return View(customerDetailsViewModel);
        }




        public IActionResult ShowAccounts(string id)
        {
            return View(_genericRepository.GetAll(id, flag: 2));
        }


        [HttpGet]
        public IActionResult Search(string search)
        {
            var tellerID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var results = _searchPaginationRepo.Search(search, tellerID);

            ViewBag.search = search;
            ViewBag.TotalRecords = results.Count();

            var cutomerstoView = _mapper.Map<List<Customer>, List<CustomersViewModel>>(results.ToList());

            return View("GetAllCustomers", cutomerstoView);
        }

    }
}
