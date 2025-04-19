using System.ComponentModel.DataAnnotations;

namespace BankingSystem.PL.ViewModels.Admin
{

    // ViewModel for creating/editing managers
    public class ManagerVM
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Address { get; set; } = null!;
        public long SSN { get; set; }
        public DateTime BirthDate { get; set; }
        public double Salary { get; set; }
        public int? BranchId { get; set; }
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string? Password { get; set; }
    }
}
