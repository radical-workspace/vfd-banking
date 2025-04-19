using BankingSystem.DAL.Models;

namespace BankingSystem.PL.ViewModels.Customer
{
    public class CustomerProfileViewModel
    {
        // customer property
        public CustomerViewModel DesiredCustomer { get; set; } 

        // account property
        public List<AccountMinimal>? Account { get; set; }

        // transaction property
        public List<TransactionMinimal>? Transactions { get; set; }

        // counts properties
        public double TotalBalance { get; set; } = 0;
        public int AccountsCount { get; set; } = 0;
        public int CardsCount { get; set; } = 0;
        public int DebitCardsCount { get; set; } = 0;
        public int CreditCardsCount { get; set; } = 0;
        public int LoansCount { get; set; } = 0;
        public int CertificatesCount { get; set; } = 0;
    }

    public class AccountMinimal
    {
        public long Number { get; set; }
        public double Balance { get; set; }
        public AccountType AccountType { get; set; }
    }

    public class TransactionMinimal
    {
        public TransactionStatus TransactionStatus { get; set; }
        public TransactionType Type { get; set; }
        public string DoneVia { get; set; } = string.Empty;
        public long? AccountDistenationNumber { get; set; }
        public double Amount { get; set; }
        public DateTime PaymentDate { get; set; }
    }
}
