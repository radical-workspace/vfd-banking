using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.DAL.Models
{
    internal class CentralBank : IBaseEntity
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public List<Branch> Branches { get; set; } = [];

        // currency : total amount
        public Dictionary<string, decimal> TotalSavings { get; set; } = null!;
        public decimal TotalLoans { get; set; }

        public List<Loan> Loans { get; set; } = [];
    }
}
