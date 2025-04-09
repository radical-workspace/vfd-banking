using BankingSystem.DAL.Models;

namespace BankingSystem.PL.ViewModels.Customer
{
    public class CustomerDetailsViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public DateTime JoinDate { get; set; } 
        public DateTime BirthDate { get; set; }
        public double? TotalBalance { get; set; }
        public List<Account>? Accounts { get; set; }
        public int AccountsCount { get; set; }
        public IEnumerable<Card> Cards { get; set; } = null!;
        public int CardsCount { get; set; }
        public int DebitCardsCount { get; set; }
        public int CreditCardsCount { get; set; }
        public List<Loan>? Loans { get; set; }
        public int LoansCount { get; set; } 
        public int CertificatCount { get; set; }
        public List<MyTransaction>? Transactions { get; set; } 

    }
}
