namespace BankingSystem.PL.ViewModels.Manager
{
    public class LoanDetail
        {
            public string LoanType { get; set; }
            public string LoanStatus { get; set; }
        }

        public class AccountDetail
        {
            public string AccountNumber { get; set; }
            public string AccountType { get; set; }
            public string AccountStatus { get; set; }
        }

        public class CardDetail
        {
            public string CardType { get; set; }
            public string Number { get; set; }
        }

        public class SupportTicketDetail
        {
            public string Status { get; set; }
            public string Type { get; set; }
        }

        public class TransactionDetail
        {
            public string TransactionType { get; set; }
        }
        public class CertificateDetail
        {
            public string CertificateNumber { get; set; } = string.Empty;
            public DateTime IssueDate { get; set; }
            public DateTime ExpiryDate { get; set; }
            public double Amount { get; set; }
            public double InterestRate { get; set; }
            public long AccountNumber { get; set; }

        }

}
