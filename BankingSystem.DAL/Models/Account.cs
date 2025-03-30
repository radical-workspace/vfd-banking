using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.DAL.Models
{
    public enum AccountType
    {
        Unauthorized,
        Savings,
        Current
    }

    public class Account : IBaseEntity
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public string AccountNumber { get; set; } = string.Empty;
        public decimal Balance { get; set; }
        public DateTime CreatedAt { get; set; }
        public AccountType Type;
        public List<Transaction> Transactions { get; set; } = [];
        public List<Loan> Loans { get; set; } = [];
    }
}
