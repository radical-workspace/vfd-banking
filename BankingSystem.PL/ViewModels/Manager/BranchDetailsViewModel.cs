using BankingSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BankingSystem.PL.ViewModels.Manager
{
    public class BranchDetailsViewModel
    {
        [Required(ErrorMessage = "Branch ID is required.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Branch Name is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Branch Name must be between 3 and 50 characters.")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Location is required.")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Location must be between 5 and 100 characters.")]
        public string Location { get; set; } = null!;

        [Required(ErrorMessage = "Opening time is required.")]
        public DateTime Opens { get; set; }

        [Required(ErrorMessage = "Closing time is required.")]
        public DateTime Closes { get; set; }

        public List<Loan>? Loans { get; set; }

        public List<Customer> Customers { get; set; } = null!;

        public List<BankingSystem.DAL.Models.Teller> Tellers { get; set; } = null!;

        public List<Department> Departments { get; set; } = null!;

        public List<Savings> Savings { get; set; } = null!;
    }
}