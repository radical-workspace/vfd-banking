using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.DAL.Models
{
    public class Manager : ApplicationUser
    {
        public double Salary { get; set; }
        public ICollection<Teller> Tellers { get; set; } = new List<Teller>();

        [ForeignKey(nameof(Branch))]
        public int? BranchId { get; set; }
        public Branch Branch { get; set; } = null!;
        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";
    }
}
