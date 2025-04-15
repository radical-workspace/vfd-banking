using BankingSystem.BLL.Interfaces;
using BankingSystem.DAL.Models;
using BankingSystem.PL.ViewModels.Customer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            var (senderAccount, validation) = GetAccountBasedOnTransferMethod(model, transaction);
            if (validation != null) return (null, null, validation)!;

            transaction.AccountId = senderAccount.Id;

            // Validate and load sender branch savings
            var (loadSuccess, error) = LoadBranchSavings(senderAccount, transaction);
            if (!loadSuccess)
                return (null, null, error);

            // Validate receiver
            var (receiverAccount, receiverError) = ValidateReceiverAccount(model.DestinationIban, transaction);
            if (receiverError != null)
                return (null, null, receiverError);

            // Set transaction details
            try
            {
                //transaction.AccountId = senderAccount.Id;
                transaction.CustomerID = senderAccount.CustomerId!;
                transaction.AccountDistenationNumber = receiverAccount.Number;

                return (senderAccount, receiverAccount, null);
            }
            catch (Exception ex)
            {
                return (null, null, FailTransfer(transaction, "System error",
                    $"Failed to set transaction details: {ex.Message}"));
            }
        }

        public ViewResult ValidateTransferRules(
            AccountsViewModel model, Account sender, Account receiver, Transaction transaction)
        {
            transaction.AccountId = sender.Id;

            if (sender.Number == receiver.Number)
                return FailTransfer(transaction, "Same account transfer", "Cannot transfer to the same account.");

            if (model.Amount > 500_000)
                return FailTransfer(transaction, "Amount exceeds limit",
                    "Transfer amount exceeds the limit. Please visit a branch");

            if (sender.Balance < model.Amount)
                return FailTransfer(transaction, "Insufficient balance", "Insufficient balance.");

            if (sender.Balance == 0 || model.Amount <= 0)
                return FailTransfer(transaction, "Invalid amount", "Invalid transfer amount.");

            if (!model.ShowAccounts)
            {
                if (sender.Card?.ExpDate < DateTime.Now)
                    return FailTransfer(transaction, "Card expired", "Card has expired.");

                if ((sender.Card?.ExpDate != model.VisaExpDate) || (sender.Card?.CVV != model.VisaCVV))
                    return FailTransfer(transaction, "Invalid Card Information ", "Double chech your Card information and try again.");
            }

            return null;
        }

        public IActionResult ExecuteTransfer(
            AccountsViewModel model, Account sender, Account receiver, Transaction transaction)
        {
            try
            {
                sender.Balance -= model.Amount;
                receiver.Balance += model.Amount;

                transaction.AccountId = sender.Id;

                var senderbranchSavings = sender.Branch.Savings.FirstOrDefault(b => b.BranchId == sender.BranchId);

                if (senderbranchSavings != null)
                {
                    senderbranchSavings.Balance -= (double)model.Amount;
                    _unitOfWork.Repository<Savings>().Update(senderbranchSavings);
                }
                else
                    //return ShowTransferError("No savings account found for branch.", "Bank error.");
                    return FailTransfer(transaction, "Bank error.", "No savings account found for branch.");

                if ((senderbranchSavings.Balance * 1.4) <= model.Amount)
                    return FailTransfer(transaction, "Bank limit exceeded", "Branch has insufficient funds for this transfer.");

                var recevierBranchSavings = receiver.Branch.Savings.FirstOrDefault(b => b.BranchId == receiver.BranchId);

                if (recevierBranchSavings != null)
                {
                    recevierBranchSavings.Balance += (double)model.Amount;
                    _unitOfWork.Repository<Savings>().Update(recevierBranchSavings);
                }
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

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////  Withdraw && Deposit ///////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public (Account myAccount, IActionResult result) GetAndValidateCurrentAccount(
                AccountsViewModel model, Transaction transaction, string UserID, bool IsUsingVisa)
        {
            var accounts = _unitOfWork.Repository<Account>()
                                        .GetAllIncluding(c => c.Customer!, a => a.Card, b => b.Branch)
                                        .Where(c => c.CustomerId == UserID)
                                        .ToList();


            if (!accounts.Any())
                return (null, ShowTransferError("No accounts found", "No accounts found"));

            var selectedAccount = IsUsingVisa ?
                                                accounts.FirstOrDefault(a => a.Card.Number.Equals(model.SelectedCardNumber))! :
                                                accounts.FirstOrDefault(a => a.Number == model.SelectedAccountNumber)!;

            if (selectedAccount == null)
                return (null, ShowTransferError("Selected card not found.", "Selected card not found."));

            if (selectedAccount.AccountStatus != AccountStatus.Active)
            {
                transaction.AccountId = selectedAccount.Id;
                return (null, FailTransfer(transaction, "Account is Inactive.", "Account is Inactive."))!;
            }
            selectedAccount.Branch.Savings = [.. _unitOfWork.Repository<Savings>().GetAll().Where(b => b.BranchId == selectedAccount.BranchId)];

            transaction.AccountId = selectedAccount.Id;
            transaction.CustomerID = selectedAccount.CustomerId!;
            transaction.AccountDistenationNumber = selectedAccount.Number;

            model.VisaExpDate = selectedAccount.Card.ExpDate;

            return (selectedAccount, null)!;
        }

        public ViewResult ValidateWithdrawlRules(
          AccountsViewModel model, Account myAccount, Transaction transaction, bool isUsingVisa)
        {
            transaction.AccountId = myAccount.Id;

            if (isUsingVisa && ((model.VisaCVV != myAccount.Card.CVV) || (model.VisaExpDate < DateTime.Now)))
            {
                return FailTransfer(transaction, "Invalid card details", "Invalid card details.");
            }
            else
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
            }

            return null;
        }

        public ViewResult ValidateDepositRules(AccountsViewModel model, Account myAccount, Transaction transaction, bool isUsingVisa)
        {

            transaction.AccountId = myAccount.Id;
            if (isUsingVisa && ((model.VisaCVV != myAccount.Card.CVV) || (model.VisaExpDate < DateTime.Now)))
                return FailTransfer(transaction, "Invalid card details", "Invalid card details.");

            else
            {
                if ((myAccount.Balance < 0) || (model.Amount <= 0))
                    return FailTransfer(transaction, "Insufficient balance", "Insufficient balance");
            }
            return null;
        }

        public IActionResult ExecuteWithdraw(
            AccountsViewModel model, Account MyAccount, Transaction transaction, bool isUsingVisa)
        {
            try
            {
                MyAccount.Balance -= model.Amount + 5; // 5 is for the bank fees

                var branchSavings = MyAccount.Branch.Savings.FirstOrDefault();
                if (branchSavings != null)
                {
                    branchSavings.Balance -= (double)model.Amount - 5;
                    _unitOfWork.Repository<Savings>().Update(branchSavings);
                }

                transaction.Status = TransactionStatus.Accepted;
                transaction.Payment.Status = PaymentStatus.Paid;
                transaction.Type = TransactionType.Withdraw;
                transaction.DoneVia = !isUsingVisa ? "Withdraw By Customer Via Account" : "Withdraw By Customer Via Visa Card";

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

        public IActionResult ExecuteDeposit(AccountsViewModel model, Account MyAccount, Transaction transaction, bool isUsingVisa)
        {
            try
            {
                if (model.SelectedDestination == AccountsViewModel.DepositDestination.Loan)
                {
                    if (!model.SelectedLoanId.HasValue)
                        return ShowTransferError("Please select a loan", "Deposit failed.");

                    var loan = _unitOfWork.Repository<Loan>().Get(model.SelectedLoanId.Value);

                    if (loan == null || loan.AccountId != MyAccount.Id)
                        return ShowTransferError("Invalid loan selection", "Deposit failed.");

                    if (model.Amount > loan.CurrentDebt)
                        return FailTransfer(transaction, "Deposit failed", "Amount exceeds loan debt");

                    if (model.Amount == loan.CurrentDebt)
                        loan.LoanStatus = LoanStatus.Paid;

                    loan.CurrentDebt -= model.Amount;
                    _unitOfWork.Repository<Loan>().Update(loan);
                }
                else
                {
                    MyAccount.Balance += model.Amount;
                    _unitOfWork.Repository<Account>().Update(MyAccount);
                }

                var branchSavings = MyAccount.Branch.Savings.FirstOrDefault();
                if (branchSavings != null)
                {
                    branchSavings.Balance += (double)model.Amount;
                    _unitOfWork.Repository<Savings>().Update(branchSavings);
                }

                transaction.Status = TransactionStatus.Accepted;
                transaction.Payment.Status = PaymentStatus.Paid;
                transaction.Type = TransactionType.Deposit;
                transaction.DoneVia = !isUsingVisa ?
                        $"Deposit By Customer Via Branch {(model.SelectedDestination == AccountsViewModel.DepositDestination.Loan ? "Loan Payment" : "Account")}" :
                        $"Deposit By Customer Via Visa Card {(model.SelectedDestination == AccountsViewModel.DepositDestination.Loan ? " (Loan Payment)" : "")}";

                _unitOfWork.Repository<Transaction>().Add(transaction);
                _unitOfWork.Complete();

                return RedirectToAction("Index", "Home");
            }
            catch
            {
                return FailTransfer(transaction, "Deposit failed", "An error occurred during deposit.");
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////      Failure         ///////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public ViewResult FailTransfer(Transaction transaction, string failureReason, string errorMessage)
        {
            // Update transaction status
            transaction.Status = TransactionStatus.Denied;
            transaction.Payment.Status = PaymentStatus.Failed;
            transaction.Payment.FailureReason = failureReason;

            // Persist the failed transaction
            _unitOfWork.Repository<Transaction>().Add(transaction);
            _unitOfWork.Complete();

            // Prepare error view model
            var errorModel = new TransactionErrorViewModel
            {
                TransactionId = transaction.Id.ToString("D8"),
                Amount = transaction.Payment.Amount,
                FailureReason = failureReason,
                ErrorMessage = errorMessage,
                ResolutionSuggestion = GetResolutionSuggestion(failureReason)
            };

            return View("TransferError", errorModel);
        }

        // Add this method to your HandleAccountTransferes class
        public IActionResult ShowTransferError(string errorMessage, string failureReason)
        {
            // Create an error model without persisting anything to database
            var errorModel = new TransactionErrorViewModel
            {
                TransactionId = "N/A", // No transaction was created
                Amount = 0, // No amount since we're not storing the transaction
                FailureReason = failureReason,
                ErrorMessage = errorMessage,
                ResolutionSuggestion = GetResolutionSuggestion(failureReason)
            };

            return View("TransferError", errorModel);
        }


        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////      Helpers         ///////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private (Account, IActionResult result) GetAccountBasedOnTransferMethod(AccountsViewModel model, Transaction transaction)
        {
            var senderAccount = _unitOfWork.Repository<Account>()
                 .GetAllIncluding(b => b.Branch, a => a.Card)
                 .FirstOrDefault(a => a.Number == model.SelectedAccountNumber);
            if (!model.ShowAccounts)
            {
                // Card transfer logic
                var senderCard = _unitOfWork.Repository<VisaCard>()
                    .GetSingleIncluding(n => n.Number == model.SelectedCardNumber, a => a.Account);

                if (senderCard == null)
                    return (null, ShowTransferError("Could not extract sender VisaCard number ", "Invalid sender VisaCard"));

                senderAccount = senderCard.Account;

            }

            if (senderAccount == null)
                return (null, ShowTransferError("Could not extract sender account number ", "Invalid Sender account number"));

            if (senderAccount.AccountStatus != AccountStatus.Active)
                return (null, FailTransfer(transaction, "Sender account is Inactive", "Account is Inactive."));

            return (senderAccount, null)!;
        }

        private (bool success, IActionResult error) LoadBranchSavings(Account account, Transaction transaction)
        {
            if (account == null)
                return (false, ShowTransferError("account is currently Invalid", "Invalid account."));

            if (account.BranchId == null)
                return (false, ShowTransferError("Account has no branch assigned", "Missing branch."));

            try
            {
                account.Branch ??= _unitOfWork.Repository<Branch>().Get((int)account.BranchId);

                if (account.Branch == null)
                    return (false, ShowTransferError("Associated branch not found", "Branch not found."));

                account.Branch.Savings = _unitOfWork.Repository<Savings>()
                    .GetAll()
                    .Where(b => b.BranchId == account.BranchId)
                    .ToList();
                if (account.Branch.Savings.Count == 0)
                {
                    transaction.AccountId = account.Id;
                    return (false,FailTransfer(transaction,"Savings Error","The Branch does not Contain the This Currency"));
                }

                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ShowTransferError("Failed to load branch savings", "System error."));
            }
        }

        private (Account receiver, IActionResult error) ValidateReceiverAccount(
     string destinationIban,
     Transaction transaction)
        {
            if (string.IsNullOrWhiteSpace(destinationIban))
                return (null, ShowTransferError("Destination IBAN is required", "Invalid IBAN"));

            var receiverAccountNumber = IbanParser.ExtractAccountNumber(destinationIban);

            if (string.IsNullOrWhiteSpace(receiverAccountNumber.ToString()))
                return (null, ShowTransferError("Could not extract account number from IBAN", "Invalid IBAN"));

            try
            {
                var receiverAccount = _unitOfWork.Repository<Account>()
                    .GetAllIncluding(b => b.Branch)
                    .FirstOrDefault(a => a.Number == receiverAccountNumber);

                if (receiverAccount == null)
                    return (null, ShowTransferError("Recipient account not found", "Invalid receiver account"));

                if (receiverAccount.AccountStatus != AccountStatus.Active)
                {
                    transaction.AccountId = receiverAccount.Id;
                    return (null, FailTransfer(transaction, "Receiver account is not active", "Inactive account"));
                }

                var (loadSuccess, error) = LoadBranchSavings(receiverAccount, transaction);
                if (!loadSuccess)
                    return (null, error);

                return (receiverAccount, null);
            }
            catch (Exception ex)
            {
                //return (null, FailTransfer(transaction, "Receiver account is not active", "Inactive account"));

                return (null, ShowTransferError($"Failed to validate receiver: {ex.Message}", "System error"));
            }
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

        private string GetResolutionSuggestion(string failureReason)
        {
            return failureReason switch
            {
                "Insufficient balance" => "Please deposit funds or try a smaller amount.",
                "Account is Inactive" => "Contact customer support to reactivate your account.",
                "Daily limit exceeded" => "Try again tomorrow or visit a branch for higher limits.",
                "Invalid receiver account" => "Verify the account details and try again.",
                _ => "Please try again or contact support if the problem persists."
            };
        }
    }
}
