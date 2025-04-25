using BankingSystem.DAL.Models;
using System.ComponentModel.DataAnnotations;

namespace BankingSystem.PL.ViewModels.Teller
{
    public class CreateCardViewModel
    {
        public int AccountId { get; set; }

       
        [StringLength(16)]
        public string Number { get; set; } = null!;

        [Required]
        [StringLength(3)]
        public string CVV { get; set; } = null!;

        
        [DataType(DataType.Date)]
        public DateTime ExpDate { get; set; }

        [Required]
        public TypeOfCard CardType { get; set; }

    }
}
