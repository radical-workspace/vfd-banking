using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.DAL.Models
{
    public enum TypeOfCard:byte
    {
        Debit=1,
        Credit=2,
    }
    public class Card:BaseEntity
    {
        public string Number { get; set; } = null!;
        public bool IsDeleted { get; set; }

        public string CVV { get; set; } = null!;

        public DateTime ExpDate { get; set; }

        public DateTime CreationDate { get; set; }

        public TypeOfCard CardType { get; set; }


    }
}
