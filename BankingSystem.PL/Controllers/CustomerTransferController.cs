using AutoMapper;
using BankingSystem.BLL.Interfaces;
using BankingSystem.DAL.Models;
using BankingSystem.PL.Helpers;
using BankingSystem.PL.ViewModels.Customer;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BankingSystem.PL.Controllers
{
    public class CustomerTransferController(IUnitOfWork unitOfWork, IMapper mapper, TransferFromAccountToAnother transfereHelper) : Controller
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly TransferFromAccountToAnother _transfereHelper = transfereHelper;


        public IActionResult TransferMoney()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return NotFound("User not found.");
            //4e30f6dc - a62d - 4012 - 8558 - fcb0594d1b3c
            var Accounts = _unitOfWork.Repository<Account>().GetAllIncluding(c => c.Customer!, a => a.Card)
                                                            .Where(c => c.CustomerId == userId).FirstOrDefault();
            if (Accounts == null) return NotFound();

            AccountsViewModel AccountsVM = _mapper.Map<AccountsViewModel>(Accounts);

            return View();
        }

        [HttpPost]
        public IActionResult TransferMoney(AccountsViewModel transferMoneyVM)
        {
            //private TransferFromAccountToAnother TransfereHelper = new TransferFromAccountToAnother(_unitOfWork);
            if (!ModelState.IsValid) return View(transferMoneyVM);

            // Initialize transaction
            var transaction = _transfereHelper.CreatePendingTransaction(transferMoneyVM, User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);

            // Get accounts
            var (senderAccount, receiverAccount, validationResult) = _transfereHelper.GetAndValidateAccounts(transferMoneyVM, transaction);
            if (validationResult != null) return validationResult;

            // Validate transfer rules
            validationResult = _transfereHelper.ValidateTransferRules(transferMoneyVM, senderAccount, receiverAccount, transaction);
            if (validationResult != null) return validationResult;

            // Execute transfer
            return _transfereHelper.ExecuteTransfer(transferMoneyVM, senderAccount, receiverAccount, transaction);
        }



    }
}
