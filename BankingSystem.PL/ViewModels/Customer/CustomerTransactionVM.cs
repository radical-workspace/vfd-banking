using BankingSystem.DAL.Models;

namespace BankingSystem.PL.ViewModels.Customer
{
    public class CustomerTransactionVM
    {
        public List<TransactionMinimal> Transactions { get; set; } = new();
        public TransactionStatus SelectedStatus { get; set; }
        public string Id { get; set; } = string.Empty;
    }
}
