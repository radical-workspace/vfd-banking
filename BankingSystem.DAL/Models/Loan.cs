using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.DAL.Models
{
    public enum LoanType
    {
        Car,
        Buisness,
        Real_State,

    }

    public enum LoanStatus
    {
        Pending,
        Denied,
        Accepted,
        Paid
    }

    public class Loan : BaseEntity
    {
        public double LoanAmount { get; set; }
        public decimal CurrentDebt { get; set; }
        public double InterestRate { get; set; }
        public bool IsDeleted { get; set; }
        public int DurationInMonth { get; set; }
        public LoanStatus LoanStatus { get; set; }
        public LoanType LoanType { get; set; }
        public DateTime StartDate { get; set; }
        public List<Payment> Payments { get; set; } = [];

        #region RelationShips

        [ForeignKey(nameof(Account))]
        public int AccountId { get; set; }
        public Account Account { get; set; } = null!;

        // i think that we should remove this relation 
        // as you will access it through the account of the user

        //[ForeignKey(nameof(Customer))]
        //public int CustomerId { get; set; }
        //public Customer Customer { get; set; } = null!;

        [ForeignKey(nameof(Branch))]
        public int BranchId { get; set; }

        public Branch Branch { get; set; } = null!;

        [ForeignKey(nameof(Teller))]
        public int TellerId { get; set; }
        public Teller Teller { get; set; } = null!;
        #endregion

    }
}
