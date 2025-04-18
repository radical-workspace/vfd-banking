using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.DAL.Models
{
    public class Branch : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string Location { get; set; } = null!;
        public TimeSpan Opens { get; set; }
        public TimeSpan Closes { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public List<Loan>? Loans { get; set; }
        public List<Customer> Customers { get; set; } = null!;
        public List<Teller> Tellers { get; set; } = null!;
        public List<Department> Departments { get; set; } = null!;
        public List<Savings> Savings { get; set; } = null!;
        public ICollection<Reservation> Reservations { get; set; } = new HashSet<Reservation>();
        //[ForeignKey(nameof(MyManager))]
        //public string ManagerId { get; set; }=string.Empty;
        public Manager ?MyManager { get; set; } = null!;

    }
}
