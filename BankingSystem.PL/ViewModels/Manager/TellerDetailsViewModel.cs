using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BankingSystem.PL.ViewModels.Manager
{
    public class TellerDetailsViewModel
    {
        public string Id { get; set; } = null!;

        [Required(ErrorMessage = "First Name is required.")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "Last Name is required.")]
        public string LastName { get; set; } = null!;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Phone Number is required.")]
        [Phone(ErrorMessage = "Invalid phone number format.")]
        public string PhoneNumber { get; set; } = null!;

        [Required(ErrorMessage = "SSN is required.")]
        [Range(10000000000000, 99999999999999, ErrorMessage = "SSN must be a 14-digit number.")]
        public long SSN { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        public string Address { get; set; } = null!;

        [Required(ErrorMessage = "Join Date is required.")]
        [DataType(DataType.Date)]
        public DateTime JoinDate { get; set; }

        [Required(ErrorMessage = "Birth Date is required.")]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "Salary is required.")]
        [Range(0.1, double.MaxValue, ErrorMessage = "Salary must be a positive number greater than 0.")]
        public double Salary { get; set; }
        public string? BranchName { get; set; }

        //public string BranchID { get; set; }
        public string? DepartmentName { get; set; }
    }
}
