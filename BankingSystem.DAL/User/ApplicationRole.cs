using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.DAL.User
{
    public class ApplicationRole
    {
        public int Id { get; set; }
        public string Title { get; set; }= string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
