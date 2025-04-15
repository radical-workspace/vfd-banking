using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace BankingSystem.DAL.Models
{
    public class GeneralCertificate : BaseEntity
    {
        public string? Name { get; set; }
        public int Duration { get; set; }
        public double InterestRate { get; set; }

        [Range(1000, double.MaxValue, ErrorMessage = "The Minimum Amount to apply in this Certificate should not be less than 1000.")]
        public double Amount { get; set; }
    }

}
