using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.DAL.Models
{
    public class Branch : BaseEntity
    {
        public string Location { get; set; } = null!;
        public bool IsDeleted { get; set; }
        public DateTime Opens { get; set; }
        public DateTime Closes { get; set; }
        public List<Loan>? Loans { get; set; }
        public List<Customer> Customers { get; set; } = [];
        public List<Teller> Tellers { get; set; } = [];
        public List<Department> Departments { get; set; } = [];
        public List<Savings> Savings  { get; set; } = null!;

        [ForeignKey(nameof(Manager))]
        public int? ManagerId { get; set; }
        public Manager? Manager { get; set; }


    }
}
