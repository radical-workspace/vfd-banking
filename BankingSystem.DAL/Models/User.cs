using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.DAL.Models
{
    public class User : BaseEntity
    {
        public int SSN { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;     
        public string Address { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public DateTime JoinDate { get; set; }
        public DateTime BirthDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
