using AutoMapper;
using BankingSystem.BLL.Interfaces;
using BankingSystem.DAL.Models;
using BankingSystem.PL.ViewModels.Manager;
using BankingSystem.PL.ViewModels.Teller;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.PL.Controllers.AppAdmin
{
    public class AdminUsersController(IUnitOfWork unitOfWork, IMapper mapper) : Controller
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;


        public ActionResult GetAllTellers()
        {
            var tellers = _unitOfWork.Repository<Teller>()
                .GetAllIncluding(c => c.Branch).ToList();

            var tellersView= _mapper.Map<List<Teller>, List<TellerDetailsViewModel>>(tellers);
            return View(tellersView);
        }


        public ActionResult GetAllCustomers()
        {
            //var customers = _unitOfWork.Repository<Customer>()
            //    .GetAllIncluding(c => c.Branch).ToList();



            var customer = _unitOfWork.Repository<Customer>().GetAllIncluding(c => c.Accounts,
                                                                         t => t.Accounts.Select(a => a.AccountTransactions),
                                                                         t => t.Accounts.Select(a => a.SupportTickets),
                                                                         t => t.Accounts.Select(a => a.Loans),
                                                                         t => t.Accounts.Select(a => a.Certificates)
                                                                         );

            var customerAccounts = customer.SelectMany(c => c.Accounts).ToList();


            ViewBag.TransactionsCount = customerAccounts.SelectMany(a => a.AccountTransactions).Count();
            ViewBag.SupportTicketsCount = customerAccounts.SelectMany(a => a.SupportTickets).Count();
            ViewBag.LoansCount = customerAccounts.SelectMany(a => a.Loans).Count();
            ViewBag.CertificatesCount = customerAccounts.SelectMany(a => a.Certificates).Count();


            var customerView = _mapper.Map<List<Customer>, List<CustomerDetailsViewModel>>(customers);
            return View(customerView);
        }
        

        public ActionResult GetAllAccounts(string customerId)
        {
            var accounts = _unitOfWork.Repository<Account>()
                .GetAllIncluding(a => a.Customer, c => c.Branch, a => a.Branch).ToList();


            


            return View(accounts);
        }

    }
}
