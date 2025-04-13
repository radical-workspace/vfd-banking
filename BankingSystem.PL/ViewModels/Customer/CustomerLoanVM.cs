using BankingSystem.DAL.Data.CustomeAttributes;
using BankingSystem.DAL.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace BankingSystem.PL.ViewModels.Customer
{
    public class CustomerLoanVM
    {
        [Required]
        public string CustomerId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Account selection is required")]
        public int? SelectedAccountId { get; set; }

        //public IEnumerable<Account> Accounts { get; set; } = [];

        public List<SelectListItem> Accounts { get; set; } = new();

       [Required(ErrorMessage = "Branch selection is required")]
        public int? BranchId { get; set; }

        [Required(ErrorMessage = "Loan amount is required")]
        [Range(1000, 1000000, ErrorMessage = "Enter a valid loan amount between 1,000 and 1,000,000")]
        public double? LoanAmount { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Debt can't be negative")]
        public double CurrentDebt { get; set; } = 0;
        
        [Required(ErrorMessage = "Interest rate is required")]
        [Range(1, 50, ErrorMessage = "Interest rate should be between 1% and 50%")]
        public int InterestRate { get; set; }

        [Required(ErrorMessage = "Loan duration is required")]
        [Range(6, 360, ErrorMessage = "Enter a valid loan term between 6 and 360 months")]
        public int DurationInMonth { get; set; }

        [Required(ErrorMessage = "Loan status is required")]
        public LoanStatus LoanStatus { get; set; } = LoanStatus.Pending;

        [Required(ErrorMessage = "Loan type is required")]
        public LoanType LoanType { get; set; } = LoanType.Other;

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Start date is required")]
        [DateAfterMonth(ErrorMessage = "Start date must be at least one month from today")]
        public DateTime StartDate { get; set; }

        // PDF Required
        //[Display(Name = "Upload Loan Document (PDF)")]
        //[Required(ErrorMessage = "You must upload a supporting document")]
        //[DataType(DataType.Upload)]
        //[FileExtensions(Extensions = "pdf", ErrorMessage = "Only PDF files are allowed")]
        //public IFormFile Document { get; set; } = null!;
    }
}
