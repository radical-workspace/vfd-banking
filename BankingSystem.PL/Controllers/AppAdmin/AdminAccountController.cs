using AutoMapper;
using BankingSystem.BLL.Interfaces;
using BankingSystem.DAL.Models;
using BankingSystem.PL.ViewModels.Admin;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.PL.Controllers.AppAdmin
{
    public class AdminAccountController(IUnitOfWork unitOfWork, IMapper mapper) : Controller
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public IActionResult AccountRelatedEntities(int Id)
        {

            var account = _unitOfWork.Repository<Account>().GetSingleIncluding(a => a.Id == Id,
                                                                                t => t.AccountTransactions,
                                                                                t => t.SupportTickets,
                                                                                t => t.Loans,
                                                                                t => t.Certificates
                                                                                );
            AccountAdminVM model = new()
            {
                TransactionsCount = account.AccountTransactions?.Count ?? 0,
                SupportTicketsCount = account.SupportTickets?.Count ?? 0,
                LoansCount = account.Loans?.Count ?? 0,
                CertificatesCount = account.Certificates?.Count ?? 0
            };
            return View(model);
        }

        public IActionResult CustomerRelatedEntities(string customerId)
        {

            var customer = _unitOfWork.Repository<Customer>().GetAllIncluding(c => c.Accounts,
                                                                                t => t.Accounts.Select(a => a.AccountTransactions),
                                                                                t => t.Accounts.Select(a => a.SupportTickets),
                                                                                t => t.Accounts.Select(a => a.Loans),
                                                                                t => t.Accounts.Select(a => a.Certificates)
                                                                                ).Where(c => c.Id == customerId);
            
            var customerAccounts = customer.SelectMany(c => c.Accounts).ToList();
            
            AccountAdminVM model = new()
            {
                TransactionsCount = customerAccounts.SelectMany(a => a.AccountTransactions).Count(),
                SupportTicketsCount = customerAccounts.SelectMany(a => a.SupportTickets).Count(),
                LoansCount = customerAccounts.SelectMany(a => a.Loans).Count(),
                CertificatesCount = customerAccounts.SelectMany(a => a.Certificates).Count()
            };

            return View(model);

        }
    }
}
