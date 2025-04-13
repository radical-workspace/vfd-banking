using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.ConstrainedExecution;

namespace BankingSystem.DAL.Models
{
    public class GeneralCertificate :BaseEntity
    {
        public string? Name { get; set; }
        public int Duration { get; set; }
        public double InterestRate { get; set; }

        [Range(1000, double.MaxValue, ErrorMessage = "the Minimum Amount To Apply in this Certificate should not be less than 1000 ")]
        public double Amount { get; set; }

    }

    public class Certificate : BaseEntity
    {
        public long CertificateNumber { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public GeneralCertificate? GeneralCertificate { get; set; } = null!;

        [ForeignKey(nameof(Account))]
        public int? AccountId { get; set; }
        public Account? Account { get; set; } = null!;
    }
}
