using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.DAL.Models
{
    public class ApplicationUser : IdentityUser, ISoftDeletable
    {
        public long  SSN { get; set; } 
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Address { get; set; } = null!;
        public DateTime JoinDate { get; set; }
        public DateTime BirthDate { get; set; }
        public string Discriminator { get; set; } = null!;
        public bool IsDeleted { get; set; }

    }

}
