using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.DAL.Models
{
    public class Manager :User
    {
        public Branch Branch { get; set; } = null!;
        public Bank  Bank{ get; set; } = null!;
    }
}
