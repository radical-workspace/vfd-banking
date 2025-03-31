using System;
using System.Collections.Generic;
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

        public decimal TotalSavings { get; set; } 

        //public List<CustomerProfile> Customers { get; set; } = [];
        //public List<EmployeeProfile> Employees { get; set; } = [];
    }
}
