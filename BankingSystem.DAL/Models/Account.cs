using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.DAL.Models
{
    public enum AccountType
    {
        Saving=1,
        Current=2
    }
    public class Account:BaseEntity
    {
        public double? Balance { get; set; }

        public bool IsDeleted { get; set; }

        public AccountType AccountType { get; set; }
        public List<Transactionn>? AccountTransactionns{ get; set; }
    }
}
