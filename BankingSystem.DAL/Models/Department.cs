using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.DAL.Models
{
    public class Department:BaseEntity
    {
        public string Name { get; set; } = null!;

        [ForeignKey(nameof(Manager))]
        public string? ManagerId{ get; set; }
        public MyManager Manager { get; set; } = null!;

        [ForeignKey(nameof(Branch))]
        public int BranchId { get; set; }
        public Branch Branch { get; set; } = null!;


    }
}
