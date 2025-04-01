using BankingSystem.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.DAL.Data.Configurations
{
    public class LoanConfiguration : IEntityTypeConfiguration<Loan>
    {
        public void Configure(EntityTypeBuilder<Loan> builder)
        {
            // Configure LoanAmount with decimal precision
            builder.Property(l => l.LoanAmount)
                .HasColumnType("decimal(18,4)")
                .IsRequired();

            // Configure Profit with decimal precision
            builder.Property(l => l.Profit)
                .HasColumnType("decimal(18,4)")
                .IsRequired();

            // Configure IsDeleted default value
            builder.Property(l => l.IsDeleted)
                .HasDefaultValue(false);

            // Store LoanType as string instead of integer
            builder.Property(l => l.LoanType)
                .HasConversion<string>();

            builder.Property(l => l.LoanStatus)
               .HasConversion<string>();

            // Default Date to current timestamp
            builder.Property(L => L.Date)
                         .HasColumnType("date");
        }
    }
}
