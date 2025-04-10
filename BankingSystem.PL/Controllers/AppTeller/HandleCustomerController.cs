using AutoMapper;
using BankingSystem.BLL.Interfaces;
using BankingSystem.BLL.Repositories;
using BankingSystem.DAL.Models;
using BankingSystem.PL.ViewModels.Teller;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Operations;
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

        public HandleCustomerController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, IMapper mapper, IGenericRepository<Account> genericRepository)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _mapper = mapper;

            //
            _genericRepository = genericRepository;
        }


        public ActionResult GetAllCustomers(string id)
        {
            var TellerHandleCustomer = _unitOfWork.Repository<Teller>().GetSingleIncluding(T => T.Id == id);
            //var TellerFromTellerTabe= 

            var branchId = TellerHandleCustomer.BranchId;


            var Customers = _unitOfWork.Repository<Customer>()
                .GetAllIncluding(C => C.Branch)
                .Where(C => C.BranchId == branchId)
                .ToList();
            var cutomerstoView = _mapper.Map<List<Customer>, List<CustomersViewModel>>(Customers);
            return View(cutomerstoView);
        }


        public ActionResult GetCustomerDetails(string id)
        {
            var Customer = _unitOfWork.Repository<Customer>()
                .GetSingleIncluding(C => C.Id == id, C => C.Branch, C => C.Loans, C => C.Transactions, C => C.Cards, C => C.SupportTickets, C => C.Accounts);

            var mappedCustomer = _mapper.Map<Customer, CustomerDetailsViewModel>(Customer);


            return View("GetCustomerDetails", mappedCustomer);
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

        // POST: HandleCustomerController/Edit/5
        [HttpPost]

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


        public ActionResult DeleteCustomer(string id)
        {
            var Customer = _unitOfWork.Repository<Customer>()
                 .GetSingleIncluding(C => C.Id == id, C => C.Branch, C => C.Loans, C => C.Transactions, C => C.Cards, C => C.SupportTickets, C => C.Accounts);

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
                  C => C.Branch, C => C.Loans, C => C.Transactions, C => C.Cards,
                  C => C.SupportTickets, C => C.Accounts);

                _unitOfWork.Repository<Customer>().Delete(customerToBeDeleted);
                _unitOfWork.Complete();
                return RedirectToAction(nameof(GetAllCustomers), new { id = User.FindFirst(ClaimTypes.NameIdentifier).Value });


            }
            return View(customerDetailsViewModel);
        }


        public IActionResult ShowAccounts(string id)
        {
            return View(_genericRepository.GetAll(id, flag: 2));
        }
    }
}
