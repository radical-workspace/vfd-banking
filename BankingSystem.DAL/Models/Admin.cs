using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.DAL.Models
{
    public class Admin : ApplicationUser
    {
        public double Salary { get; set; }
        [ForeignKey(nameof(Bank))]
        public int? BankId { get; set; }
        public Bank Bank { get; set; } = null!;
    }
}
