using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BankingSystem.PL.ViewModels
{
    [Keyless]
    public class LoginUserViewModel
    {
        [Display(Name ="Email Address")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Enter Your Email Address")]
        public string Email { get; set; } = null!;

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Enter Your Password")]

        public string Password { get; set; } = null!;

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }

    }
}
