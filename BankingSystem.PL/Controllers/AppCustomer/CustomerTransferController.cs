using AutoMapper;
using BankingSystem.BLL.Interfaces;
using BankingSystem.DAL.Models;
using BankingSystem.PL.Helpers;
using BankingSystem.PL.ViewModels.Customer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace BankingSystem.PL.Controllers.AppCustomer
{
    public class CustomerTransferController(IUnitOfWork unitOfWork, IMapper mapper, HandleAccountTransferes transfereHelper) : Controller
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly HandleAccountTransferes _transfereHelper = transfereHelper;

        public IActionResult TransferMoney()
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

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult TransferMoney(AccountsViewModel transferMoneyVM)
        {
            if (!ModelState.IsValid)
            {
                // Repopulate dropdowns
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var accounts = _unitOfWork.Repository<Account>()
                    .GetAllIncluding(c => c.Customer!, a => a.Card)
                    .Where(c => c.CustomerId == userId)
                    .ToList();

                transferMoneyVM.UserAccounts = accounts.Select(a => new SelectListItem
                {
                    Value = a.Number.ToString(),
                    Text = $"Account: {a.Number} - Balance: {a.Balance:C}"
                }).ToList();

                transferMoneyVM.UserVisaCards = accounts.Where(a => a.Card != null)
                    .Select(c => new SelectListItem
                    {
                        Value = c.Card!.Number,
                        Text = $"Card: {c.Card.Number}"
                    }).ToList();

                return View(transferMoneyVM);
            }
            // Initialize transaction
            var transaction = _transfereHelper.CreatePendingTransaction(transferMoneyVM, User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);

            // Get accounts
            var (senderAccount, receiverAccount, validationResult) = _transfereHelper.GetAndValidateAccounts(transferMoneyVM, transaction);
            
            if (validationResult != null) return validationResult;
                //return _transfereHelper.ShowTransferError("One or both of the use accounts is invalid.", "Account invalid");

            // Validate transfer rules
            validationResult = _transfereHelper.ValidateTransferRules(transferMoneyVM, senderAccount, receiverAccount, transaction);
            if (validationResult != null) return validationResult;
            //return _transfereHelper.ShowTransferError("One or both of the use accounts is invalid.", "Account invalid");

            // Execute transfer
            return _transfereHelper.ExecuteTransfer(transferMoneyVM, senderAccount, receiverAccount, transaction);
        }
    }
}
