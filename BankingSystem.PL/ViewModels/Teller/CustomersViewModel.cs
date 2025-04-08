using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BankingSystem.PL.ViewModels.Teller
{
   
    public class CustomersViewModel
    {
      
        public string Id { get; set; } = null!;
        [Required (ErrorMessage ="SSN Is Required")]
        public long SSN { get; set; }
        [Display(Name ="First Name")]
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
    }
}
