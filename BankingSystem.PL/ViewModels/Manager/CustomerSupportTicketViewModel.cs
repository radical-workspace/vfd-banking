using BankingSystem.DAL.Models;

namespace BankingSystem.PL.ViewModels.Manager
{
    public class CustomerSupportTicketViewModel
    {
        public long AccountNumber { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public SupportTicketStatus Status { get; set; }
        public SupportTicketType Type { get; set; }
        public string Response { get; set; }
    }
}
