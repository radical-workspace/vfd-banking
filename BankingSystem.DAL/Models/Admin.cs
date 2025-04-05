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
        [ForeignKey(nameof(Branch))]
        public int? BranchId { get; set; }
        public Branch Branch { get; set; } = null!;
    }
}
