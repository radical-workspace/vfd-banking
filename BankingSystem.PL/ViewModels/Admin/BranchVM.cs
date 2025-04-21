
// BranchViewModel.cs
using System.ComponentModel.DataAnnotations;

namespace BankingSystem.PL.ViewModels
{
    public class BranchVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Branch name is required")]
        [StringLength(100, ErrorMessage = "Branch name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Location is required")]
        [StringLength(200, ErrorMessage = "Location cannot exceed 200 characters")]
        public string Location { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bank ID is required")]
        public int BankId { get; set; }
        public bool IsDeleted { get; set; }

        [Required(ErrorMessage = "Opening time is required")]
        [Display(Name = "Opens At")]
        [DataType(DataType.Time)]
        public TimeSpan Opens { get; set; }

        [Required(ErrorMessage = "Closing time is required")]
        [Display(Name = "Closes At")]
        [DataType(DataType.Time)]
        public TimeSpan Closes { get; set; }

        public string? ManagerId { get; set; }
        public string? ManagerFirstName { get; set; }
        public string? ManagerLastName { get; set; }
        public string? ManagerEmail { get; set; }
        public long? ManagerSSN { get; set; }
    }
}