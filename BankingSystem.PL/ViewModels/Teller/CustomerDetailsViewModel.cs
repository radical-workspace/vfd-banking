using BankingSystem.DAL.Models;
using System.ComponentModel.DataAnnotations;

namespace BankingSystem.PL.ViewModels.Teller
{
    public class CustomerDetailsViewModel
    {
        public string Id { get; set; } = null!;
        [Required(ErrorMessage = "SSN Is Required")]
        public long SSN { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = null!;
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = null!;
        [Display(Name = "Branch Name")]
        public string Branch { get; set; } = null!;
        public string Address { get; set; } = null!;
        [Display(Name = "Join Date")]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime JoinDate { get; set; }
        [Display(Name = "Birth Date")]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime BirthDate { get; set; }

        // Direct navigation entities
        public List<Account>? Accounts { get; set; }
        public List<Loan>? Loans { get; set; }
        public List<Transaction>? Transactions { get; set; }
        public List<SupportTicket>? Tickets { get; set; }

        // Count properties
        public int TransactionsCount { get; set; }
        public int SupportTicketsCount { get; set; }
        public int LoansCount { get; set; }
        public int CertificatesCount { get; set; }
    }
}
