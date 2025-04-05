using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.DAL.Models
{
    public class Teller : ApplicationUser
    {
        [ForeignKey(nameof(Branch))]
        public int? BranchId { get; set; }
        public Branch Branch { get; set; } = null!;
        [ForeignKey(nameof(Department))]
        public int? DeptId { get; set; }
        public Department Department { get; set; } = null!;
    }
}
