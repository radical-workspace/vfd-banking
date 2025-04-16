using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.DAL.Models
{
    public enum TransactionType
    {
        Deposit,
        Withdraw,
        Transfer,
        LoanPayment
    }
    public enum TransactionStatus
    {
        Pending,
        Denied,
        Accepted
    }

    public class Transaction : BaseEntity
    {
        public TransactionStatus Status { get; set; }
        public TransactionType Type { get; set; }
        public string DoneVia { get; set; } = string.Empty;
        public long? AccountDistenationNumber { get; set; }

        [ForeignKey(nameof(Payment))]
        public int PaymentId { get; set; }
        public required Payment Payment { get; set; }

        [ForeignKey(nameof(Customer))]
        public string CustomerID { get; set; }
        public Customer Customer { get; set; }

        [ForeignKey(nameof(Account))]
        public int AccountId { get; set; }
        public Account Account { get; set; } = null!;
    }
}
