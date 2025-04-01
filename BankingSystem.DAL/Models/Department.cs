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
        public bool IsDeleted { get; set; }


        [ForeignKey(nameof(Manager))]
        public int ManagerId{ get; set; }
        public Manager Manager { get; set; } = null!;   

    }
}
