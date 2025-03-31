using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.DAL.Models
{
    public  class Bank:BaseEntity
    {
        public string Name { get; set; } = null!;
        public string CentralAddress { get; set; } = null!;
    }
}
