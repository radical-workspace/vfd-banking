using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace BankingSystem.PL.ViewModels.Auth
{
    [Keyless]
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        public string Email { get; set; }
    }

}
