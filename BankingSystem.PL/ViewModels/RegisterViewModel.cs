using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BankingSystem.PL.ViewModels
{
    [Keyless]
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Enter Your SSN")]
        [RegularExpression(@"^\d{13}$", ErrorMessage = "SSN must be exactly 13 digits")]
        public long SSN { get; set; }


        [Required(ErrorMessage ="Enter First Name")]
        [Display(Name ="First Name")]
        [MaxLength(10)]
        [MinLength(5)]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "Enter Last Name")]
        [Display(Name = "Last Name")]
        [MaxLength(10)]
        [MinLength(5)]
        public string LastName { get; set; } = null!;


        [Required(ErrorMessage = "Enter User Name")]
        [Display(Name = "User Name")]
        [MaxLength(10)]
        [MinLength(5)]
        public string UserName { get; set; } = null!;


        [Required(ErrorMessage = "Enter Phone Number")]
        [Display(Name = "Phone Number")]
        [MaxLength(11)]
        [MinLength(11)]
        public string PhoneNumber { get; set; } = null!;

        [Required(ErrorMessage ="Enter Your Address")]
        public string Address { get; set; } = null!;

        [Required(ErrorMessage = "Enter Your Birth Date")]
        [DataType(DataType.Date)]
        [Display(Name = "Birth Date")]
        public DateTime BirthDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Join Date")]
        public DateTime JoinDate { get; set; }

        [Required(ErrorMessage ="Enter Your Email Address")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email Address")]

        public string Email { get; set; } = null!;

        [DataType(DataType.Password)]
        [Required(ErrorMessage ="Enter Your Password")]
        public string Password { get; set; } = null!;

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; } = null!;

        [Required (ErrorMessage ="Please Select Your Role")]
        [Display(Name = "Select Role")]
        public string Role { get; set; } = null!;

        public List<SelectListItem>? AvailableRoles { get; set; }



    }
}
