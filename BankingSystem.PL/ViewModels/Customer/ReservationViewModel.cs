using System.ComponentModel.DataAnnotations;
using BankingSystem.DAL.Models;
using BankingSystem.PL.Validation;

namespace BankingSystem.PL.ViewModels.Customer
{
    public class ReservationViewModel
    {
        [Required(ErrorMessage = "Branch is required.")]
        [Display(Name = "Branch")]
        public int BranchId { get; set; }

        [Display(Name = "Reservation Date")]
        [Required(ErrorMessage = "Reservation date is required.")]
        [DataType(DataType.DateTime)]
        [ValidReservationDate]
        public DateTime ReservationDate { get; set; }

        [Required(ErrorMessage = "Service type is required.")]
        [Display(Name = "Service Type")]
        public ServiceType ServiceType { get; set; }

        [StringLength(500, ErrorMessage = "Notes can't exceed 500 characters.")]
        [Display(Name = "Notes")]
        public string Notes { get; set; }
    }
}
