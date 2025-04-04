using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.DAL.Models
{
    public enum TransationType
    {
        Deposit = 1,
        Withdraw = 2,
        Transfer = 3
    }
    public enum TransationStatus
    {
        Pending,
        Denied,
        Accepted
    }

    public class MyTransaction : BaseEntity
    {
        public TransationStatus Status { get; set; }
        public TransationType Type { get; set; }
        public string DoneVia { get; set; } = string.Empty;
        
        [ForeignKey(nameof(Payment))]
        public int PaymentId { get; set; } // FK
        public required Payment Payment { get; set; }

        [ForeignKey(nameof(Account))]
        public int? AccountId { get; set; }
        public Account? Account { get; set; }
    }
}
