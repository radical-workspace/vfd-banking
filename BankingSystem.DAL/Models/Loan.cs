using System;
using System.Collections.Generic;
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
    public class Loan :BaseEntity
    {
        public double LoanAmount { get; set; }

        public double Profit { get; set; }
        public bool  IsDeleted { get; set; }

        public LoanType LoanType { get; set; }

        public DateTime Date { get; set; }
    }
}
