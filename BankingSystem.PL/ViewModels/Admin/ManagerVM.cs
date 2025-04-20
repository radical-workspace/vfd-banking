using System.ComponentModel.DataAnnotations;

namespace BankingSystem.PL.ViewModels.Admin
{

    // ViewModel for creating/editing managers
    public class ManagerVM
    {
        public string Id { get; set; }

        [Required]
        public string FirstName { get; set; } = null!;

        [Required]
        public string LastName { get; set; } = null!;

        [Required]
        public string Address { get; set; } = null!;

        [Required]
        public long SSN { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        [Required]
        public double Salary { get; set; }

        [Required]
        public int? BranchId { get; set; }

        [Required]
        public string Email { get; set; } = null!;

        [Required]
        public string PhoneNumber { get; set; } = null!;

        [Required]
        public string? Password { get; set; }
    }
}
