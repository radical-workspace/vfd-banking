using BankingSystem.DAL.Models;

namespace BankingSystem.PL.Controllers
{
    public class AccountsViewModel
    {
        public int Id { get; set; }
        public double? Balance { get; set; }
        public long AccountNumber { get; set; }
        public long Iban { get; set; }

        public string VisaNumber { get; set; } = null!;

        public string VisaCVV { get; set; } = null!;

        public DateTime VisaExpDate { get; set; }

    }
}