using BankingSystem.DAL.Models;

namespace BankingSystem.PL.ViewModels.Manager
{
    public class LoanDetailsViewModel
    {
        public int Id { get; set; }
        public  LoansViewModel? Loan { get; set; }
        public List<PaymentList>? Payments { get; set; }
        //public List<IncomeSource>? AdditionalIncomeSources { get; set; }
        //public List<Asset>? Assets { get; set; }

        public List<FinancialDocument>? FinancialDocument { get; set; } // This will be used to store the financial documents of the customer

    }

    public class PaymentList
    {
        public double Amount { get; set; }
        public DateTime PaymentDate { get; set; }
    }
}
