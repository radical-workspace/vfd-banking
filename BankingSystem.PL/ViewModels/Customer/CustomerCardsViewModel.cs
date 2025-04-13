using BankingSystem.DAL.Models;

namespace BankingSystem.PL.ViewModels.Customer
{
    public class CustomerCardsViewModel
    {
        public string Number { get; set; } = null!;

        public string CVV { get; set; } = null!;

        public DateTime ExpDate { get; set; }

        public DateTime CreationDate { get; set; }

        public TypeOfCard CardType { get; set; }

    }
}
