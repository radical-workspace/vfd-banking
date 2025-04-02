using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.DAL.Models
{
    public class Savings : BaseEntity
    {
        public bool IsDeleted { get; set; }
        [ForeignKey(nameof(Branch))]
        public int BracketId { get; set; }
        public Branch Branch { get; set; } = null!;
        public string Currency { get; set; }=null!;
        public decimal Balance{ get; set; }

    }
}
