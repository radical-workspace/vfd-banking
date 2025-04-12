using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Principal;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.DAL.Models
{
    public enum AccountType
    {
        Saving = 1,
        Current = 2
    }
    public enum AccountStatus
    {
        Active = 1,
        Inactive = 2,
        Closed = 3
    }
    public class Account : BaseEntity
    {
        public long Number { get; set; }
        public double? Balance { get; set; }
        public DateTime CreatedAt { get; set; }
        public AccountType AccountType { get; set; }
        public AccountStatus AccountStatus { get; set; }
        public List<Transaction>? AccountTransactions { get; set; }
        public List<Certificate> Certificates { get; set; } = [];

        public List<Loan> Loans { get; set; } = [];
        public List<VisaCard> Cards { get; set; } = [];
        public List<SupportTicket> SupportTickets { get; set; } = [];

        [ForeignKey(nameof(Customer))]
        public string? CustomerId { get; set; }
        public Customer? Customer { get; set; } = null!;

        [ForeignKey(nameof(Branch))]
        public int? BranchId { get; set; }
        public Branch? Branch { get; set; } = null!;

    }
}
