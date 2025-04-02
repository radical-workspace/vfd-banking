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
    public class Transaction:BaseEntity
    {
        public double Amount { get; set; }

        public DateTime Date { get; set; }

        public string Status { get; set; } = null!;

        public bool IsDeleted{ get; set; }

        public TransationType Type { get; set; }

        [ForeignKey(nameof(Account))]

        public int? AccountId { get; set; }

        public Account? Account { get; set; }
    }
}
