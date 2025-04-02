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

    public class MyTransaction:BaseEntity
    {
        public double Amount { get; set; }

        public DateTime Date { get; set; }

        public TransationStatus Status { get; set; }

        public bool IsDeleted{ get; set; }

        public TransationType Type { get; set; }

        [ForeignKey(nameof(Account))]

        public int? AccountId { get; set; }

        public Account? Account { get; set; }
    }
}
