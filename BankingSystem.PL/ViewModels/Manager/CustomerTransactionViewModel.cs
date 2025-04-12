using BankingSystem.DAL.Models;

namespace BankingSystem.PL.ViewModels.Manager
{
    public class CustomerTransactionViewModel
    {
        public long AccountNumber { get; set; }
        public long AccountDestinatoin { get; set; }
        public TransationStatus Status { get; set; }
        public TransationType Type { get; set; }
        public string DoneVia { get; set; } = string.Empty;
    }
}
