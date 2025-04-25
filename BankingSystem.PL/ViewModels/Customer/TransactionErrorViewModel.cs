namespace BankingSystem.PL.ViewModels.Customer
{
    public class TransactionErrorViewModel
    {
        public string TransactionId { get; set; }
        public double Amount { get; set; }
        public string FailureReason { get; set; }
        public string ErrorMessage { get; set; }
        public string ResolutionSuggestion { get; set; }
    }
}