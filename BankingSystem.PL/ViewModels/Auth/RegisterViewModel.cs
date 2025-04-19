using BankingSystem.PL.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BankingSystem.PL.ViewModels.Auth
{
    [Keyless]
    public class RegisterViewModel
    {
        public string? Id { get; set; }
        // Personal Information
        [UniqueSSN]
        [Required(ErrorMessage = "Enter Your SSN")]
        [RegularExpression(@"^\d{14}$", ErrorMessage = "SSN must be exactly 14 digits")]
        public long SSN { get; set; }

        [Required(ErrorMessage = "Enter First Name")]
        [Display(Name = "First Name")]
        [StringLength(10, MinimumLength = 3, ErrorMessage = "First Name must be between 3 and 10 characters")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "Enter Last Name")]
        [Display(Name = "Last Name")]
        [StringLength(10, MinimumLength = 3, ErrorMessage = "Last Name must be between 3 and 10 characters")]
        public string LastName { get; set; } = null!;

        [Required(ErrorMessage = "Enter Your Birth Date")]
        [DataType(DataType.Date)]
        [Display(Name = "Birth Date")]
        public DateTime BirthDate { get; set; } = DateTime.Now.AddYears(-25).AddMonths(-10).AddDays(-10);

        [Required(ErrorMessage = "Enter Your Address")]
        public string Address { get; set; } = null!;

        // Contact Info
        [Required(ErrorMessage = "Enter Phone Number")]
        [Display(Name = "Phone Number")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "Phone Number must be exactly 11 digits")]
        [UniquePhoneNumber]
        public string PhoneNumber { get; set; } = null!;

        [Required(ErrorMessage = "Enter Your Email Address")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email Address")]
        [UniqueEmail]
        public string Email { get; set; } = null!;

        // Account Info
        [Required(ErrorMessage = "Enter User Name")]
        [Display(Name = "User Name")]
        [StringLength(10, MinimumLength = 3, ErrorMessage = "User Name must be between 3 and 10 characters")]
        public string UserName { get; set; } = null!;

        [Required(ErrorMessage = "Enter Your Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Confirm Your Password")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare(nameof(Password), ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; } = null!;

        [Required(ErrorMessage = "Enter Join Date")]
        [DataType(DataType.Date)]
        [Display(Name = "Join Date")]
        public DateTime JoinDate { get; set; } = DateTime.Now;

        // Role Info
        [Required(ErrorMessage = "Please Select Your Role")]
        [Display(Name = "Select Role")]
        public string Role { get; set; } = null!;

        [Required(ErrorMessage = "Enter Salary")]
        [Range(15000, double.MaxValue, ErrorMessage = "Salary must be a more than 15000 EGP")]
        public double Salary { get; set; }
        public List<SelectListItem>? AvailableRoles { get; set; }
    }
}