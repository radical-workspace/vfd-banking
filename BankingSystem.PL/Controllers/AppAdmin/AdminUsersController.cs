using AutoMapper;
using BankingSystem.BLL.Interfaces;
using BankingSystem.DAL.Models;
using BankingSystem.PL.ViewModels.Manager;
using BankingSystem.PL.ViewModels.Teller;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

            var tellersView = _mapper.Map<List<Teller>, List<TellerDetailsViewModel>>(tellers);
            return View(tellersView);
        }


        public ActionResult GetAllCustomers()
        {
            var customers = _unitOfWork.Repository<Customer>()
                .GetAll()
                .Include(c => c.Branch)
                .Include(c => c.Accounts)
                    .ThenInclude(a => a.AccountTransactions)
                .Include(c => c.Accounts)
                    .ThenInclude(a => a.SupportTickets)
                .Include(c => c.Accounts)
                    .ThenInclude(a => a.Loans)
                .Include(c => c.Accounts)
                    .ThenInclude(a => a.Certificates)
                .Include(c => c.Transactions)
                .Include(c => c.Loans)
                .Include(c => c.SupportTickets)
                .ToList();

            var customerView = _mapper.Map<List<Customer>, List<CustomerDetailsViewModel>>(customers);

            return View(customerView);
        }

        public ActionResult GetAllAccounts(string customerId)
        {
            var accounts = _unitOfWork.Repository<Account>()
                .GetAllIncluding(a => a.Customer,c => c.Certificates, 
                                    a => a.SupportTickets, a => a.AccountTransactions,
                                    a => a.Branch, a => a.Loans).ToList();
            return View(accounts);
        }
    }
}
