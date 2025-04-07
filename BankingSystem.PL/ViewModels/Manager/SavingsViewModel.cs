using BankingSystem.DAL.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankingSystem.PL.ViewModels.Manager
{
    public class SavingsViewModel
    {
        public int Id { get; set; } 
        public string Currency { get; set; } = null!;
        public double Balance { get; set; }
        public string BranchName { get; set; } = string.Empty; // will need in the admin panel
    }
}
