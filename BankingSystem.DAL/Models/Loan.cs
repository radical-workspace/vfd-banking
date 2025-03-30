using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.DAL.Models
{
    public class Loan : IBaseEntity
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public string Details { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public decimal InterestRate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime CreatedAt { get; set; }

        //added new payment [ time : Amount]
        public Dictionary<DateTime, decimal> UpdatedAt { get; set; } = new Dictionary<DateTime, decimal>();
        
        [ForeignKey(nameof(Account))]
        public int AccountId { get; set; }
        public Account Account { get; set; } = null!;
    }
}
