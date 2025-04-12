namespace BankingSystem.PL.ViewModels.Manager
{
    public class LoanViewModel
    {
        public long AccountNumber { get; set; }
        public double LoanAmount { get; set; }
        public double CurrentDebt { get; set; }
        public int InterestRate { get; set; }
        public int DurationInMonth { get; set; }
        public string LoanStatus { get; set; } = string.Empty;
        public string LoanType { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public List<PaymentViewModel> Payments { get; set; } = new();
    }


}
