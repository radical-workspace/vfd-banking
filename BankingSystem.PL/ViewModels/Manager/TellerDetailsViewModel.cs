using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BankingSystem.PL.ViewModels.Manager
{
    public class TellerDetailsViewModel
    {
        public string Id { get; set; } = null!;

        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Phone Number is required.")]
        [Phone(ErrorMessage = "Invalid phone number format.")]
        public string PhoneNumber { get; set; } = null!;

        public string? BranchName { get; set; }

        public List<SelectListItem> Branches { get; set; } = new();

        [Required(ErrorMessage = "Branch selection is required.")]
        public int BranchID { get; set; }
    }
}
