using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.DAL.Models
{
    public class Customer :User
    {

        public List<Transaction>? Transactions { get; set; }
       
        public List<Loan>? Loans { get; set; }
        public List<Account>? Accounts { get; set; }

        [ForeignKey(nameof(Card))]
        public int CardId { get; set; }
        Card? Card { get; set; }


    }
}
