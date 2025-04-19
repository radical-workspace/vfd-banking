using BankingSystem.DAL.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

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
        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters")]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "Description is required")]
        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
        public string Description { get; set; } = null!;

        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Status is required")]
        public SupportTicketStatus Status { get; set; }

        [Required(ErrorMessage = "Please select an issue type")]
        public SupportTicketType Type { get; set; }

        public string? Response { get; set; }

        public string? CustomerId { get; set; }

        [Required(ErrorMessage = "Please select an account related to this issue")]
        [Display(Name = "Related Account")]
        public int? SelectedAccountId { get; set; }

        public List<SelectListItem> Accounts { get; set; } = new();
    }


}
