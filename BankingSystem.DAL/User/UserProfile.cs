using BankingSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.DAL.User
{
    public abstract class BaseProfile : IBaseEntity
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser User { get; set; } = null!;
        public bool IsDeleted { get; set; }
    }
    public  class CustomerProfile : BaseProfile
    {

        public string AccountNumber { get; set; } = string.Empty;
        public decimal Balance { get; set; }

        public string Category { get; set; } = string.Empty;

        [ForeignKey(nameof(Branch))]
        public int BranchId { get; set; }
        public Branch EnrolledBranch { get; set; } = null!;
    }

    public abstract class EmployeeProfile : BaseProfile
    {
        public string EmployeeId { get; set; } = string.Empty;

        [ForeignKey(nameof(Branch))]
        public int BranchId { get; set; }
        public string Branch { get; set; } = string.Empty;
    }

    public class ManagerProfile : EmployeeProfile
    {

    }
    public class TellerProfile : EmployeeProfile
    {
    }

    public class CustomerServiceProfile : EmployeeProfile
    {
    }

    public class HRProfile : EmployeeProfile
    {
    }
}
