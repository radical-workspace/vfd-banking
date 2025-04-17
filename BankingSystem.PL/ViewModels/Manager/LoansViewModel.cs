using BankingSystem.DAL.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankingSystem.PL.ViewModels.Manager
{
    public class LoansViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required(ErrorMessage = "Loan amount is required.")]
        [Range(1, double.MaxValue, ErrorMessage = "Loan amount must be greater than 0.")]
        public double LoanAmount { get; set; }

        [Required(ErrorMessage = "Current debt is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Current debt cannot be negative.")]
        public double CurrentDebt { get; set; }

        [Required(ErrorMessage = "Interest rate is required.")]
        [Range(1, 100, ErrorMessage = "Interest rate must be between 1% and 100%.")]
        public int InterestRate { get; set; }

        [Required(ErrorMessage = "Duration is required.")]
        [Range(1, 120, ErrorMessage = "Duration must be between 1 and 120 months.")]
        public int DurationInMonth { get; set; }

        [Required(ErrorMessage = "Loan status is required.")]
        public LoanStatus LoanStatus { get; set; }

        [Required(ErrorMessage = "Loan type is required.")]
        public LoanType LoanType { get; set; }

        [Required(ErrorMessage = "Start date is required.")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Account number is required.")]
        [Range(1, long.MaxValue, ErrorMessage = "Account number must be valid.")]
        public long AccountNumber { get; set; }

        [Required(ErrorMessage = "Customer name is required.")]
        [StringLength(100, ErrorMessage = "Customer name cannot exceed 100 characters.")]
        public string? CustomerName { get; set; }
        public string SSN { get; set; } = string.Empty;

    }
}
