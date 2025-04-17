namespace BankingSystem.PL.ViewModels.Manager
{
    public class CustomerCardViewModel
    {
        public long AccountNumber { get; set; }
        public string Number { get; set; } = null!;
        public string CVV { get; set; } = null!;
        public DateTime ExpDate { get; set; }
        public DateTime CreationDate { get; set; }
        public string CardType { get; set; } = null!;
    }
}
