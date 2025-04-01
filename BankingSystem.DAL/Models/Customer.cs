using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.DAL.Models
{
    public class Customer :User
    {
        public List<Transactionn>? Transactionns { get; set; }
       
        public List<Loan>? Loans { get; set; } 
    }
}
