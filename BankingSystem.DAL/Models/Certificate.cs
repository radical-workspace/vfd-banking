using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankingSystem.DAL.Models
{

    [Index(nameof(CertificateNumber), IsUnique = true)]
    public class Certificate : BaseEntity
    {
        public string CertificateNumber { get; set; } //string not long
        public DateTime IssueDate { get; set; }
        public DateTime ExpiryDate { get; set; }

        public double? Amount { get; set; }

        [ForeignKey(nameof(GeneralCertificate))] 
        public int? GeneralCertificateId { get; set; }
        public GeneralCertificate? GeneralCertificate { get; set; }

        [ForeignKey(nameof(Account))]
        public int? AccountId { get; set; }
        public Account? Account { get; set; } = null!;
    }
   
}
