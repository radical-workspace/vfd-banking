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
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
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
        private readonly IGenericRepository<Account> _genericRepositoryAcc;
        private readonly IGenericRepository<VisaCard> _genericRepositoryCard;
        private readonly ISearchPaginationRepo<Customer> _searchPaginationRepo;

        public HandleCustomerController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, IMapper mapper,
            IGenericRepository<Account> genericRepository, IGenericRepository<VisaCard> genericRepositoryCard, ISearchPaginationRepo<Customer> searchPaginationRepo)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _mapper = mapper;

            //
            _genericRepositoryAcc = genericRepository;
            _searchPaginationRepo = searchPaginationRepo;
            _genericRepositoryCard = genericRepositoryCard;
        }


        public ActionResult GetAllCustomers(string id, string? filter)
        {
            var TellerHandleCustomer = _unitOfWork.Repository<Teller>().GetSingleIncluding(T => T.Id == id);
            //var TellerFromTellerTabe= 

            var branchId = TellerHandleCustomer?.BranchId;


            var Customers = _unitOfWork.Repository<Customer>()
                .GetAllIncluding(C => C.Branch)


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

            var Customerr = _unitOfWork.Repository<Customer>()
            .GetAllIncluding(
                c => c.Transactions,
                c => c.Loans,
                c => c.SupportTickets,
                c => c.Accounts,
                c => c.Branch
            )
            .FirstOrDefault(c => c.Id == id);

            if (Customerr == null)
            {
                return NotFound("Customer not found");
            }

            Customerr.Accounts = _unitOfWork.Repository<Account>()
                .GetAllIncluding(a => a.Card)
                .Where(a => a.CustomerId == id)
                .ToList();

            var mappedCustomer = _mapper.Map<Customer, CustomerDetailsViewModel>(Customerr);

            return View("GetCustomerDetails", mappedCustomer);
        }


        public ActionResult CreateCustomer()
        {
            ViewData["FixedRole"] = "Customer";
            return View("~/Views/Account/Register.cshtml");
        }

        [HttpPost]
        public async Task<ActionResult> CreateCustomer(RegisterViewModel UserToRegister)
        {
            ViewData["FixedRole"] = "Customer";

            var tellerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var TellerHandleCustomer = _unitOfWork.Repository<Teller>().GetSingleIncluding(t => t.Id == tellerId);

            if (UserToRegister is not null)
            {
                ModelState.Remove("Salary");

                if (ModelState.IsValid)
                {
                    IdentityResult result;
                    ApplicationUser appUser;

                    if (UserToRegister.Role == "Customer")
                    {
                        UserToRegister.Id = Guid.NewGuid().ToString();
                        var customer = _mapper.Map<Customer>(UserToRegister);
                        customer.Id = UserToRegister.Id;

                        customer.BranchId = TellerHandleCustomer.BranchId;

                        result = await _userManager.CreateAsync(customer, UserToRegister.Password);
                        appUser = customer;
                    }
                    else
                    {
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
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
            }

            return View(nameof(Register), UserToRegister);
        }

        public ActionResult EditCustomer(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var customer = _userManager.Users.OfType<Customer>().FirstOrDefault(c => c.Id == id);
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
                var customer = _userManager.Users.OfType<Customer>().FirstOrDefault(c => c.Id == id);
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
            var Customer = _unitOfWork.Repository<Customer>()
                 .GetSingleIncluding(C => C.Id == id, C => C.Branch, C => C.Loans, C => C.Transactions, /*C => C.Cards,*/ C => C.SupportTickets, C => C.Accounts);

            var mappedCustomerToDeleted = _mapper.Map<Customer, CustomerDetailsViewModel>(Customer);


            return View(mappedCustomerToDeleted);
        }

        [HttpPost]
        public ActionResult DeleteCustomer(CustomerDetailsViewModel customerDetailsViewModel)
        {

            if (customerDetailsViewModel is not null)
            {
                var customerToBeDeleted = _unitOfWork.Repository<Customer>()
                  .GetSingleIncluding(C => C.Id == customerDetailsViewModel.Id,
                  C => C.Branch, C => C.Loans, C => C.Transactions, /*C => C.Cards,*/
                  C => C.SupportTickets, C => C.Accounts);



                if (customerToBeDeleted != null)
                    if (customerToBeDeleted.Accounts != null)
                    {
                        var accs = _unitOfWork.Repository<Account>().GetAllIncluding(a => a.Card)
                                 .Where(a => a.CustomerId == customerToBeDeleted.Id)
                                 .ToList();

                        foreach (var acc in accs)
                            _unitOfWork.Repository<Account>().Delete(acc);
                    }



                _unitOfWork.Repository<Customer>().Delete(customerToBeDeleted);
                _unitOfWork.Complete();
                return RedirectToAction(nameof(GetAllCustomers), new { id = User.FindFirst(ClaimTypes.NameIdentifier).Value });


            }
            return View(customerDetailsViewModel);
        }

        public IActionResult ShowAccounts(string id)
        {
            return View(_genericRepositoryAcc.GetAll(id, flag: 2));
        }


        public IActionResult ShowCards(string id)
        {
            return View(_genericRepositoryCard.GetAll(id));
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
