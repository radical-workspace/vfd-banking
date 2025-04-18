using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.DAL.Models
{
    public enum SupportTicketStatus
    {
        Pending,
        Denied,
        InProgress,
        Resolved
    }

    public enum SupportTicketType
    {
        AccountIssue,
        LoanIssue,
        TransactionIssue,
        Other
    }

    public class SupportTicket : BaseEntity
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime Date { get; set; }
        public SupportTicketStatus Status { get; set; }
        public SupportTicketType Type { get; set; }
        public string? Response { get; set; }

        [ForeignKey(nameof(Customer))]
        public string? CustomerId { get; set; }
        public Customer? Customer { get; set; }

        [ForeignKey(nameof(Teller))]
        public string? TellerId { get; set; }
        public Teller? Teller { get; set; }

        [ForeignKey(nameof(Account))]
        public int? AccountId { get; set; }
        public Account? Account { get; set; }

    }
}
