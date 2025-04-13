namespace BankingSystem.PL.ViewModels.Manager
{
    public class ManagerCustomerDetailsViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SSN { get; set; }
        public string BranchName { get; set; }
        public DateTime JoinDate { get; set; }
        public DateTime BirthDate { get; set; }

        public List<LoanDetail> LoanDetails { get; set; } = new List<LoanDetail> { new LoanDetail { LoanType = "No loans available", LoanStatus = "" } };
        public List<AccountDetail> AccountDetails { get; set; } = new List<AccountDetail> { new AccountDetail { AccountNumber = "No accounts available", AccountType = "", AccountStatus = "" } };
        public List<CardDetail> CardDetails { get; set; } = new List<CardDetail> { new CardDetail { CardType = "No cards available", Number = "" } };
        public List<SupportTicketDetail> SupportTicketDetails { get; set; } = new List<SupportTicketDetail> { new SupportTicketDetail { Status = "No support tickets available", Type = "" } };
        public List<TransactionDetail> TransactionDetails { get; set; } = new List<TransactionDetail> { new TransactionDetail { TransactionType = "No transactions available" } };
        public List<CertificateDetail> CertificateDetails { get; set; } = new List<CertificateDetail> { new CertificateDetail { CertificateNumber = "No Certificate available" } };

    }
}
