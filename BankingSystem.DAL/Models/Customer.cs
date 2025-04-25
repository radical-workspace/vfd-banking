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
        public List<Transaction>? Transactions { get; set; }
        public List<Loan>? Loans { get; set; }
        public List<Account>? Accounts { get; set; }
        //public IEnumerable<VisaCard> Cards { get; set; } = null!;
        public List<SupportTicket>? SupportTickets { get; set; } = null!;
        public ICollection<Reservation> Reservations { get; set; } = new HashSet<Reservation>();


        // Additional properties for financial assessment needed Whenever the customer applies for a loan
        //public List<IncomeSource>? AdditionalIncomeSources { get; set; }
        //public List<Asset>? Assets { get; set; } 

        //[ForeignKey(nameof(FinancialDocument))]
        //public int FinancialDocumentID { get; set; }
        public List<FinancialDocument> FinancialDocument { get; set; } = null!;
        //public FinancialDocument? FinancialDocument { get; set; } // This will be used to store the financial documents of the customer
    }


}
