using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.DAL.Models
{
    public enum PaymentStatus
    {
        Pending,
        Paid,
        Failed
    }

    public class Payment : BaseEntity
    {
        public double Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public PaymentStatus Status { get; set; }

        public string? FailureReason { get; set; }


        [ForeignKey(nameof(Loan))]
        public int? LoanId { get; set; }  // FK
        public Loan? Loan { get; set; }
    }
}
