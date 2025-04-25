using System.ComponentModel.DataAnnotations;

namespace BankingSystem.PL.ViewModels.Customer
{
    public class CustomerViewModel
    {
        public string Id { get; set; } = string.Empty;

        [Required(ErrorMessage = "Enter First Name")]
        [Display(Name = "First Name")]
        [StringLength(10, MinimumLength = 3, ErrorMessage = "First Name must be between 3 and 10 characters")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Enter Last Name")]
        [Display(Name = "Last Name")]
        [StringLength(10, MinimumLength = 3, ErrorMessage = "Last Name must be between 3 and 10 characters")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Enter Your Email Address")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email Address")]
        public string Email { get; set; } = string.Empty;

        // Contact Info
        [Required(ErrorMessage = "Enter Phone Number")]
        [Display(Name = "Phone Number")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "Phone Number must be exactly 11 digits")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Enter Your Address")]
        [StringLength(50)]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "Enter Join Date")]
        [DataType(DataType.Date)]
        [Display(Name = "Join Date")]
        public DateTime JoinDate { get; set; }

        [Required(ErrorMessage = "Enter Your Birth Date")]
        [DataType(DataType.Date)]
        [Display(Name = "Birth Date")]
        public DateTime BirthDate { get; set; }
    }
}
