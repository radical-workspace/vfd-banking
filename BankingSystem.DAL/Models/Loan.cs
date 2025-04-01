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
        Pending = 0,
        Denied=1,
        Accepted=2
    }
    public class Loan :BaseEntity
    {
        public double LoanAmount { get; set; }

        public LoanStatus  LoanStatus{ get; set; }
        public double Profit { get; set; }
        public bool  IsDeleted { get; set; }

        public LoanType LoanType { get; set; }
        public DateTime Date { get; set; }


        #region RelationShips

        [ForeignKey(nameof(Customer))]
        public int CustomerId { get; set; }
        public Customer Customer { get; set; } = null!;

        [ForeignKey(nameof(Branch))]
        public int BranchId { get; set; }

        public Branch Branch { get; set; } = null!;

        [ForeignKey(nameof(Teller))]
        public int TellerId { get; set; }
        public Teller Teller { get; set; }=null!;
        #endregion



    }
}
