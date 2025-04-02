using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Principal;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.DAL.Models
{
    public enum AccountType
    {
        Saving=1,
        Current=2
    }
    public class Account:BaseEntity
    {
        public double? Balance { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreatedAt { get; set; }
        public AccountType AccountType { get; set; }
        public List<MyTransaction>? AccountTransactionns{ get; set; }
        public List<Certificate> Certificates { get; set; } = [];

        public List<Loan> Loans { get; set; } = [] ;
   
        [ForeignKey(nameof(Customer))]
        public int? CustomerId { get; set; }
        public Customer Customer { get; set; } = null!;


        public Card Card { get; set; } = null!;

    }
}
