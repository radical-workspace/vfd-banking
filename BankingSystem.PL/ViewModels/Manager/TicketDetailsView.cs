using BankingSystem.DAL.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankingSystem.PL.ViewModels.Manager
{
    public class TicketDetailsView
    {
        public int Id { get; set; }
        public TicketsViewModel Ticket { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string? Response { get; set; }
        //public FinancialDocument? Document { get; set; } = null!;

    }
}


