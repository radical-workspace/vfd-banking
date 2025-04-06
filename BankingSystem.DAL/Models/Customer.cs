using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.DAL.Models
{
    public class Customer : ApplicationUser
    {
        [ForeignKey(nameof(Branch))]
        public int? BranchId { get; set; }
        public Branch Branch { get; set; } = null!;
        public List<MyTransaction>? Transactions { get; set; }
        public List<Loan>? Loans { get; set; }
        public List<Account>? Accounts { get; set; }
        public IEnumerable<Card> Cards { get; set; } = null!;
        public List<SupportTicket>? SupportTickets { get; set; } = null!;
    }
}
