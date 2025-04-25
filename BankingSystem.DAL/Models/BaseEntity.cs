using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.DAL.Models
{
    public interface ISoftDeletable
    {
        bool IsDeleted { get; set; } 
    }

    public class BaseEntity : ISoftDeletable
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
    }
}
