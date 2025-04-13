using BankingSystem.DAL.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;

namespace BankingSystem.PL.ViewModels.Teller
{
    public class CardsViewModel
    {
        public int Id { get; set; }
        [Display(Name ="Card Number")]
        public string Number { get; set; } = null!;

        [Display(Name ="Card CVV")]
        public string CVV { get; set; } = null!;

        [Display(Name = "Expiration Date")]

        public DateTime ExpDate { get; set; }
        [Display(Name = "Creation Date")]

        public DateTime CreationDate { get; set; }
        [Display(Name ="Card Type")]
        public string CardType { get; set; } = null!;

        [Display(Name = "Customer Name")]
        public string CustomerName { get; set; } = null!;

        [Display(Name ="Account Number")]
        public long AccountNumber { get; set; } 
    }
}
