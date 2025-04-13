using BankingSystem.BLL.Interfaces;
using BankingSystem.DAL.Models;
using BankingSystem.PL.ViewModels.Customer;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;
using System.Security.Claims;

namespace BankingSystem.PL.Helpers
{
    public class HandleAccountTransferes(IUnitOfWork unitOfWork) : Controller
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public (Account sender, Account receiver, IActionResult result) GetAndValidateAccounts(
    AccountsViewModel model, Transaction transaction)
        {
            var senderAccount = _unitOfWork.Repository<Account>().GetAllIncluding()
                                                                    .Where(s => s.Number == model.AccountNumber)
                                                                    .FirstOrDefault(a => a.Number == model.AccountNumber);

            if (senderAccount == null)
                return (null, null, FailTransfer(transaction, "Invalid sender account", "Invalid senderAccount details."))!;


            var receiverAccountNumber = IbanParser.ExtractAccountNumber(model.DestinationIban);
            var receiverAccount = _unitOfWork.Repository<Account>().GetAllIncluding()
                                                                    .Where(s => s.Number == receiverAccountNumber)
                                                                    .FirstOrDefault(a => a.Number == receiverAccountNumber);

            if (receiverAccount == null)
                return (null, null, FailTransfer(transaction, "Invalid receiver account", "Invalid receiverAccount details."))!;


            transaction.AccountId = senderAccount.Id;
            transaction.CustomerID = senderAccount.CustomerId!;
            transaction.AccountDistenationNumber = receiverAccountNumber;

            return (senderAccount, receiverAccount, null)!;
        }


        public (Account myAccount, IActionResult result) GetAndValidateCurrentAccount(AccountsViewModel model, Transaction transaction)
        {
            var accounts = _unitOfWork.Repository<Account>()
                            .GetAllIncluding(c => c.Customer!, a => a.Card)
                            .Where(c => c.CustomerId == User.FindFirst(ClaimTypes.NameIdentifier)?.Value!)
                            .ToList();

            if (!accounts.Any())
                return (null, FailTransfer(transaction, "No accounts found", "No accounts found."))!;
            var selectedAccount = accounts.FirstOrDefault(a => a.Number == model.SelectedAccountNumber);

            if (selectedAccount == null)
                return (null, FailTransfer(transaction, "Selected account not found", "Selected account not found."))!;

            transaction.AccountId = selectedAccount.Id;
            transaction.CustomerID = selectedAccount.CustomerId!;
            transaction.AccountDistenationNumber = selectedAccount.Number;

            return (selectedAccount, null)!;
        }

        public ViewResult ValidateTransferRules(
            AccountsViewModel model, Account sender, Account receiver, Transaction transaction)
        {
            if (sender.Number == receiver.Number)
                return FailTransfer(transaction, "Same account transfer", "Cannot transfer to the same account.");

            if (model.Amount > 500_000)
                return FailTransfer(transaction, "Amount exceeds limit",
                    "Transfer amount exceeds the limit. Please visit a branch");

            if (sender.Balance < model.Amount)
                return FailTransfer(transaction, "Insufficient balance", "Insufficient balance.");

            if (sender.Balance == 0 || model.Amount == 0)
                return FailTransfer(transaction, "Invalid amount", "Invalid transfer amount.");

            return null;
        }


        public ViewResult ValidateWithdrawlRules(
            AccountsViewModel model, Account myAccount, Transaction transaction)
        {
            if ((myAccount.Balance <= 0) || (model.Amount <= 0))
                return FailTransfer(transaction, "Insufficient balance",
                                                 "Insufficient balance");

            else if ((model.Amount % 50) != 0)
                return FailTransfer(transaction, "Can only withdraw 50 EGP Or it's multipliers.",
                                                 "Can only withdraw 50 EGP Or it's multipliers.");

            else if (myAccount.Balance < (model.Amount + 5)) // 5 is for the bank fees
                return FailTransfer(transaction, "Insufficient balance", "Insufficient balance.");

            else if (model.Amount > 25_000)
                return FailTransfer(transaction, "AWithdraw amount exceeds the limit. Please visit a branch.",
                                                 "Withdraw amount exceeds the limit. Please visit a branch.");

            return null;
        }

        public IActionResult ExecuteTransfer(
            AccountsViewModel model, Account sender, Account receiver, Transaction transaction)
        {
            try
            {
                sender.Balance -= model.Amount;
                receiver.Balance += model.Amount;

                transaction.Status = TransactionStatus.Accepted;
                transaction.Payment.Status = PaymentStatus.Paid;

                _unitOfWork.Repository<Account>().Update(sender);
                _unitOfWork.Repository<Account>().Update(receiver);
                _unitOfWork.Repository<Transaction>().Add(transaction);
                _unitOfWork.Complete();

                return RedirectToAction("Index", "Home");
            }
            catch
            {
                return FailTransfer(transaction, "Transfer failed", "An error occurred during transfer.");
            }
        }
        public IActionResult ExecuteWithdraw(
            AccountsViewModel model, Account MyAccount, Transaction transaction
            )
        {
            try
            {
                MyAccount.Balance -= model.Amount + 5; // 5 is for the bank fees
                transaction.Status = TransactionStatus.Accepted;
                transaction.Payment.Status = PaymentStatus.Paid;
                transaction.Type = TransactionType.Withdraw;
                transaction.DoneVia = "Withdraw By Customer";

                _unitOfWork.Repository<Account>().Update(MyAccount);
                _unitOfWork.Repository<Transaction>().Add(transaction);
                _unitOfWork.Complete();

                return RedirectToAction("Index", "Home");
            }
            catch
            {
                return FailTransfer(transaction, "Withdraw failed", "An error occurred during withdraw.");
            }
        }

        public ViewResult FailTransfer(Transaction transaction, string failureReason, string errorMessage)
        {
            transaction.Status = TransactionStatus.Denied;
            transaction.Payment.Status = PaymentStatus.Failed;
            transaction.Payment.FailureReason = failureReason;

            _unitOfWork.Repository<Transaction>().Add(transaction);
            _unitOfWork.Complete();

            ModelState.AddModelError("", errorMessage);
            return View("TransferMoney");
        }

        public Transaction CreatePendingTransaction(AccountsViewModel model, string uID)
        {
            return new Transaction
            {
                CustomerID = uID,
                Status = TransactionStatus.Pending,
                Type = TransactionType.Transfer,
                DoneVia = "Transfer By Customer",
                Payment = new Payment
                {
                    Amount = (double)model.Amount!,
                    PaymentDate = DateTime.Now,
                    Status = PaymentStatus.Pending
                }
            };
        }

    }
}
