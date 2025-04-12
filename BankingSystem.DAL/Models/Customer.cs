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
        public IEnumerable<VisaCard> Cards { get; set; } = null!;
        public List<SupportTicket>? SupportTickets { get; set; } = null!;

        // Additional properties for financial assessment needed Whenever the customer applies for a loan
        //public List<IncomeSource>? AdditionalIncomeSources { get; set; }
        //public List<Asset>? Assets { get; set; } 

        //[ForeignKey(nameof(FinancialDocument))]
        //public int FinancialDocumentID { get; set; }
        public FinancialDocument? FinancialDocument { get; set; } // This will be used to store the financial documents of the customer
    }

    public class IncomeSource : BaseEntity
    {
        public string Type { get; set; } = string.Empty; // "Rental", "Bonus", etc.        
        public double MonthlyAmount { get; set; }
    }

    public class Asset : BaseEntity
    {
        public string Type { get; set; } = string.Empty;  // "Property", "Vehicle", etc.
        public double EstimatedValue { get; set; }
    }

    public class FinancialDocument : BaseEntity
    {
        public string DocumentType { get; set; } = string.Empty; // "IncomeStatement", "AssetDeclaration", "TaxReturn"
        public string Description { get; set; } = string.Empty;
        public DateTime IssueDate { get; set; }
        public byte[] FileData { get; set; } = null!; // PDF content
        public string FileName { get; set; } = string.Empty;
        public string ContentType { get; set; } = "application/pdf";
        public long FileSize { get; set; }

        [ForeignKey(nameof(Customer))]
        public string CustomerId { get; set; } = string.Empty;
        public Customer Customer { get; set; } = null!;
    }

}
