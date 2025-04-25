using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.DAL.Models
{
    public enum ReservationStatus
    {
        Pending,
        Approved,
        Rejected
    }
    public enum ServiceType
    {
        OpenAccount,
        CashWithdrawal,
        Deposit,
        CardIssuance,
        LoanInquiry,
        CurrencyExchange,
        Other
    }

    [Table("Reservations")]
    public class Reservation : BaseEntity
    {
        public DateTime ReservationDate { get; set; }
        public ServiceType ServiceType { get; set; }
        public string Notes { get; set; } = string.Empty;
        [Required(ErrorMessage = "Status is Required")]
        public ReservationStatus Status { get; set; } = ReservationStatus.Pending;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        //[Required(ErrorMessage = "Customer is required. To create an account, please visit the branch directly without booking a reservation.")]
        //[Required(ErrorMessage = "Customer is required.")]
        [Display(Name = "Customer")]
        [ForeignKey(nameof(Customer))]
        public string? CustomerId { get; set; }
        public Customer? Customer { get; set; }

        [ForeignKey(nameof(Branch))]
        public int BranchId { get; set; }
        public Branch Branch { get; set; }
    }

}
