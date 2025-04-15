using BankingSystem.DAL.Models;
using BankingSystem.DAL.Data.CustomeAttributes;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace BankingSystem.PL.ViewModels.Customer
{
    public class CustomerCertificateVM
    {
        [Required]
        public string CustomerId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Account selection is required")]
        public int SelectedAccountId { get; set; }
        public List<SelectListItem> Accounts { get; set; } = new();

        public int SelectedCertificateId { get; set; }
        public List<GeneralCertificate> Certificates { get; set; } = new();

        public double? Amount { get; set; }
        public int CustomerCertificateId { get; set; }

        public DateTime IssueDate { get; set; }
        public DateTime ExpiryDate { get; set; }

        public string? Name { get; set; }
        public int Duration { get; set; }
        public double InterestRate { get; set; }
        public string CustomerCertificateNumber { get; set; }


    }
}
