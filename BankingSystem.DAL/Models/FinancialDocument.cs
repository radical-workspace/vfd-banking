using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.DAL.Models
{
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
