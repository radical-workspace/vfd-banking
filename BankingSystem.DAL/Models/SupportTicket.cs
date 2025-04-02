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
        Accepted,
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
        public bool IsDeleted { get; set; }
        public DateTime Date { get; set; }
        public SupportTicketStatus Status { get; set; }
        public SupportTicketType Type { get; set; }
        public string? Response { get; set; }

        [ForeignKey(nameof(Customer))]
        public int? CustomerId { get; set; }
        public Customer? Customer { get; set; }
    }
}
