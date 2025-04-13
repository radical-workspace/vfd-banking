using BankingSystem.BLL.Interfaces;
using BankingSystem.DAL.Models;
using BankingSystem.PL.ViewModels.Customer;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.PL.Helpers
{
    public class TransferFromAccountToAnother(IUnitOfWork unitOfWork) :Controller
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
