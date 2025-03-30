using BankingSystem.DAL.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.DAL.Models
{
    public class Branch : IBaseEntity
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public string Location { get; set; } = string.Empty;
        public TimeOnly OpenAt { get; set; }
        public DateTime CloseAt { get; set; }

        public DateTime CreatedAt { get; set; }
        public Dictionary<string, decimal> TotalSavings { get; set; } = null!;

        public List<CustomerProfile> Customers { get; set; } = [];
        public List<EmployeeProfile> Employees { get; set; } = [];
    }
}
