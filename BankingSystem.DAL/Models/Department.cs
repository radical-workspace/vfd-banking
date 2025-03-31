using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.DAL.Models
{
    public class Department:BaseEntity
    {
        public string Name { get; set; } = null!;
        public bool IsDeleted { get; set; }

    }
}
