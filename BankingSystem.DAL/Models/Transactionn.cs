using System;
using System.Collections.Generic;
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
    }
}
