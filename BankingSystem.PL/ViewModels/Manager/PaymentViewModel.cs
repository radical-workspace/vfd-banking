namespace BankingSystem.PL.ViewModels.Manager
{
    public class PaymentViewModel
    {
        public double Amount { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime PaymentDate { get; set; }
    }

}
