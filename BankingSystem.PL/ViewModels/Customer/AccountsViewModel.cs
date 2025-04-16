using BankingSystem.DAL.Models;
using BankingSystem.PL.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace BankingSystem.PL.ViewModels.Customer
{
    public class AccountsViewModel
    {
        public int Id { get; set; }
        // Common fields
        [Required(ErrorMessage = "Amount is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        public double Amount { get; set; }

        [Required(ErrorMessage = "Destination IBAN is required")]
        [StringLength(34, MinimumLength = 15, ErrorMessage = "IBAN must be between 15-34 characters")]
        public string DestinationIban { get; set; }

        // Account transfer fields
        [RequiredWhen(nameof(ShowAccounts), true, ErrorMessage = "Please select an account")]
        public long? SelectedAccountNumber { get; set; }
        public List<SelectListItem> UserAccounts { get; set; } = new();

        // For card selection
        [Display(Name = "Card")]
        [RequiredWhen(nameof(ShowAccounts), false, ErrorMessage = "Please select a card")]
        [CreditCard(ErrorMessage = "Invalid card number")]
        public string? SelectedCardNumber { get; set; }

        // Add this property
        public List<SelectListItem> UserVisaCards { get; set; } = new List<SelectListItem>();

        [RequiredWhen(nameof(ShowAccounts), false, ErrorMessage = "CVV is required")]
        [StringLength(4, MinimumLength = 3, ErrorMessage = "CVV must be 3-4 digits")]
        [RegularExpression(@"^\d{3,4}$", ErrorMessage = "CVV must be numeric")]
        public string? VisaCVV { get; set; }

        [RequiredWhen(nameof(ShowAccounts), false, ErrorMessage = "Expiration date is required")]
        //[FutureDate(ErrorMessage = "Card must not be expired")]
        public DateTime? VisaExpDate { get; set; }

        // UI state
        public bool ShowAccounts { get; set; } = true;

        // Loan payment fields (if needed)
        public DepositDestination SelectedDestination { get; set; }
        public int? SelectedLoanId { get; set; }
        public IEnumerable<SelectListItem> AvailableLoans { get; set; } = new List<SelectListItem>();

        public enum DepositDestination
        {
            [Display(Name = "Account")]
            Account,
            [Display(Name = "Loan")]
            Loan
        }
    }
}