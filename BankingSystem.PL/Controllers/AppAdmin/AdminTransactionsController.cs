using AutoMapper;
using BankingSystem.BLL.Interfaces;
using BankingSystem.DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.PL.Controllers.AppAdmin
{
    public class AdminTransactionsController(IUnitOfWork unitOfWork) : Controller
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;


        [HttpGet]
        public IActionResult GetAllTransactionsBranches()
        {
            var transactions = _unitOfWork.Repository<Transaction>().GetAllIncluding(t => t.Account, t => t.Payment, t => t.Customer)
                .Where(t => t.Type == TransactionType.Withdraw || t.Type == TransactionType.Deposit || t.Type == TransactionType.LoanPayment);
            
            return View(transactions);
        }


        [HttpGet]
        public IActionResult GetLastTransactionsForBranch(int id)
        {
            var branchTransactions = _unitOfWork.Repository<Transaction>()
                .GetAllIncluding(t => t.Customer)
                .Where(t => t.Customer.BranchId == id &&
                            (t.Type == TransactionType.Withdraw || t.Type == TransactionType.Deposit || t.Type == TransactionType.LoanPayment))
                .OrderByDescending(t => t.Payment.PaymentDate)
                .ToList();

            return View(branchTransactions);
        }
    }
}
