using BankingSystem.DAL.Models;

namespace BankingSystem.PL.ViewModels.Customer
{
    public class CustomerSupportTicketsViewModel
    {
        public List<CustomerSupportTicket> Tickets { get; set; } = new ();
        public SupportTicketStatus SelectedStatus { get; set; } 
        public string Id { get; set; } = string.Empty;

    }

    public class CustomerSupportTicket
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime Date { get; set; }
        public SupportTicketStatus Status { get; set; }
        public SupportTicketType Type { get; set; }
        public string? Response { get; set; }
    }
}
