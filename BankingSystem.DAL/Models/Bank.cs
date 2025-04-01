using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.DAL.Models
{
    public  class Bank:BaseEntity
    {
        public string Name { get; set; } = null!;
        public string CentralAddress { get; set; } = null!;
        public int ManagerId { get; set; }

        [ForeignKey(nameof(ManagerId))]
        public Manager Manager { get; set; } = null!; 
    }
}
