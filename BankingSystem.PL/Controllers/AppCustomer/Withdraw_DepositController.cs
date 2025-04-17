using BankingSystem.BLL.Interfaces;
using BankingSystem.DAL.Models;
using BankingSystem.PL.Helpers;
using BankingSystem.PL.ViewModels.Customer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace BankingSystem.PL.Controllers.AppCustomer
{
    public class Withdraw_DepositController(IUnitOfWork unitOfWork, HandleAccountTransferes transference) : Controller
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly HandleAccountTransferes _transference = transference;

        public IActionResult Withdraw()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return NotFound("User not found.");

            var accounts = _unitOfWork.Repository<Account>()
                                        .GetAllIncluding(c => c.Customer!, a => a.Card)
                                        .Where(c => c.CustomerId == userId)
                                        .ToList();

            if (!accounts.Any()) return NotFound("No accounts found.");

            var viewModel = new AccountsViewModel
            {
                // Map accounts to SelectListItems
                UserAccounts = [.. accounts.Select(a => new SelectListItem
                {
                    Value = a.Number.ToString(),
                    Text = $"Account: {a.Number} - Balance: {a.Balance:C}"
                })],
                UserVisaCards = [.. accounts.Select(c=> new SelectListItem {
                    Value = c.Card!.Number.ToString(),
                    Text = $"Card : {c.Card.Number} - Balance: {c.Balance:C}"
                })],
                ShowAccounts = true
            };
            ViewBag.Process = TransactionType.Withdraw;
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Withdraw(AccountsViewModel model, bool IsUsingVisa)
        {

            if (!ModelState.IsValid) return View(model);
            var transaction = _transference.CreatePendingTransaction(model, User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);

            // Get the selected account
            var (MyAccount, ValidationResult) = _transference.GetAndValidateCurrentAccount(model, transaction, User.FindFirst(ClaimTypes.NameIdentifier)?.Value!, IsUsingVisa);
            if (ValidationResult != null) return ValidationResult;

            // Validate the withdrawal rules
            var verifyWithdrawl = _transference.ValidateWithdrawlRules(model, MyAccount, transaction, IsUsingVisa);
            if (verifyWithdrawl != null) return verifyWithdrawl;

            // Execute the withdrawal
            return _transference.ExecuteWithdraw(model, MyAccount, transaction, IsUsingVisa);
        }

        public IActionResult Deposit()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return NotFound("User not found.");

            var accounts = _unitOfWork.Repository<Account>()
                                    .GetAllIncluding(c => c.Customer!, a => a.Card, a => a.Loans)
                                    .Where(c => c.CustomerId == userId)
                                    .ToList();

            if (!accounts.Any()) return NotFound("No accounts found.");

            var activeLoans = accounts.SelectMany(a => a.Loans)
                                         .Where(l => l.LoanStatus == LoanStatus.Accepted && l.CurrentDebt > 0)
                                         .ToList();

            var viewModel = new AccountsViewModel
            {
                UserAccounts = accounts.Select(a => new SelectListItem
                {
                    Value = a.Number.ToString(),
                    Text = $"Account: {a.Number} - Balance: {a.Balance:C}"
                }).ToList(),

                UserVisaCards = accounts.Where(a => a.Card != null)
                                       .Select(c => new SelectListItem
                                       {
                                           Value = c.Card!.Number.ToString(),
                                           Text = $"Card : {c.Card.Number} - Balance: {c.Balance:C}"
                                       }).ToList(),

                AvailableLoans = activeLoans.Select(l => new SelectListItem
                {
                    Value = l.Id.ToString(),
                    Text = $"{l.LoanType} Loan - Remaining: {l.CurrentDebt:C}"
                }),

                ShowAccounts = true
            };

            ViewBag.Process = TransactionType.Deposit;
            return View("~/Views/Withdraw_Deposit/Withdraw.cshtml", viewModel);
        }

        [HttpPost]
        public IActionResult Deposit(AccountsViewModel model, bool IsUsingVisa)
        {
            if (!ModelState.IsValid) return View(model);
            var transaction = _transference.CreatePendingTransaction(model, User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            // Get the selected account
            var (MyAccount, ValidationResult) = _transference.GetAndValidateCurrentAccount(model, transaction, User.FindFirst(ClaimTypes.NameIdentifier)?.Value!, IsUsingVisa);
            if (ValidationResult != null) return ValidationResult;
            // Validate the deposit rules
            var verifyDeposit = _transference.ValidateDepositRules(model, MyAccount, transaction, IsUsingVisa);
            if (verifyDeposit != null) return verifyDeposit;
            // Execute the deposit
            return _transference.ExecuteDeposit(model, MyAccount, transaction, IsUsingVisa);
        }
    }
}

