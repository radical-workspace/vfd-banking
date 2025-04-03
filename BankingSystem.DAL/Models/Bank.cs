using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.DAL.Models
{
    public class Bank : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string CentralAddress { get; set; } = null!;


        [ForeignKey(nameof(Manager))]
        public string ManagerId { get; set; } = null!;
        public Manager Manager { get; set; } = null!;
        public List<Branch> Branches { get; set; } = [];
    }
}
