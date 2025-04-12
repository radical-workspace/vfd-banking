namespace BankingSystem.PL.ViewModels.Manager
{
    public class CustomerAccountViewModel
    {
        public long Number { get; set; }
        public double? Balance { get; set; }
        public DateTime CreatedAt { get; set; }
        public string AccountType { get; set; }
        public string AccountStatus { get; set; }
    }

}
