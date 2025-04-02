using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankingSystem.DAL.Models
{
    public class Certificate : BaseEntity
    {
        public string CertificateNumber { get; set; } = string.Empty;
        public DateTime IssueDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public double Amount { get; set; }
        public bool IsDeleted { get; set; }
        public double InterestRate { get; set; }

        [ForeignKey(nameof(Account))]
        public int AccountId { get; set; }
        public Account Account { get; set; } = null!;
    }
}
