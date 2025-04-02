using BankingSystem.DAL.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.DAL.User
{
    public class ApplicationUser : IdentityUser
    {
        public int SSN { get; set; }
        public string Address { get; set; } = string.Empty;
        public DateTime JoinedAt { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string FullName { get; set; } = string.Empty;


        // Navigation property for banking-specific data
        public CustomerProfile CustomerProfile { get; set; }=null!;
        public TellerProfile TellerProfile { get; set; } = null!;
        public ManagerProfile ManagerProfile { get; set; } = null!;
    }
}
