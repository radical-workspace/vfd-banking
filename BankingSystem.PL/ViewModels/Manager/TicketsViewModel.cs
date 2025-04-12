using BankingSystem.DAL.Models;

namespace BankingSystem.PL.ViewModels.Manager
{
    public class TicketsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public DateTime Date { get; set; }
        public SupportTicketStatus Status { get; set; }
        public SupportTicketType Type { get; set; }
        public string? CustomerName { get; set; }
        public long CustomerAccountNumber { get; set; }
        public string? TellerName { get; set; }

    }
}
