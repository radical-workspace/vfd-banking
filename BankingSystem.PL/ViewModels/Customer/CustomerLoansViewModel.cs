using BankingSystem.DAL.Models;

namespace BankingSystem.PL.ViewModels.Customer
{
    public class CustomerLoansViewModel
    {
        public double LoanAmount { get; set; }
        public double CurrentDebt { get; set; }
        public int InterestRate { get; set; }
        public int DurationInMonth { get; set; }
        public LoanStatus LoanStatus { get; set; }
        public LoanType LoanType { get; set; }
        public DateTime StartDate { get; set; }
    }
}
