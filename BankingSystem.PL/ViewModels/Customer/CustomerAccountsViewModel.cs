using BankingSystem.DAL.Models;

namespace BankingSystem.PL.ViewModels.Customer
{
    public class CustomerAccountsViewModel
    {
        public long Number { get; set; }
        public double? Balance { get; set; }
        public DateTime CreatedAt { get; set; }
        public AccountType AccountType { get; set; }
        public AccountStatus AccountStatus { get; set; }
    }
}
