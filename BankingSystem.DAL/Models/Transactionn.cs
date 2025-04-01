using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.DAL.Models
{
    public class Transactionn:BaseEntity
    {
        public double Amount { get; set; }

        public DateTime Date { get; set; }

        public string Status { get; set; } = null!;


        public bool IsDeleted{ get; set; }

        [ForeignKey(nameof(Customer))]
        public int CustomerId { get; set; }
        public Customer Customer { get; set; } = null!;

        [ForeignKey(nameof(Account))]

        public int? AccountId { get; set; }

        public Account? Account { get; set; }
    }
}
