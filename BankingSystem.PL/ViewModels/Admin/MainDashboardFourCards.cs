using BankingSystem.DAL.Models;

namespace BankingSystem.PL.ViewModels.Admin
{
    public class MainDashboardFourCards
    {
        public int TodayTransactions { get; set; }
        public int Branches { get; set; }
        public int ActiveAccounts { get; set; }
        public int Holdings { get; set; }
        public List<Transaction> Transactions{ get; set; } = [];
    }
}
