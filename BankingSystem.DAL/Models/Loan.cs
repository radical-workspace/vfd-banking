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
        Business,
        Real_State,
        Other

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
        public double CurrentDebt { get; set; }
        public int InterestRate { get; set; }
        public int DurationInMonth { get; set; }
        public LoanStatus LoanStatus { get; set; }
        public LoanType LoanType { get; set; }
        public DateTime StartDate { get; set; }
        public List<Payment> Payments { get; set; } = null!;
        public List<FinancialDocument>? FinancialDocument { get; set; } // This will be used to store the financial documents of the customer


        #region RelationShips

        [ForeignKey(nameof(Account))]
        public int AccountId { get; set; }
        public Account Account { get; set; } = null!;


        [ForeignKey(nameof(Customer))]
        public string? CustomerId { get; set; }
        public Customer Customer { get; set; } = null!;


        [ForeignKey(nameof(Branch))]
        public int BranchId { get; set; }
        public Branch Branch { get; set; } = null!;

    
        #endregion

    }
}
