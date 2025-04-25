using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BankingSystem.DAL.Models;
using System.Reflection.Emit;

namespace BankingSystem.DAL.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder
            .HasOne(c => c.Branch)
            .WithMany(b => b.Customers)
            .HasForeignKey(c => c.BranchId)
            .OnDelete(DeleteBehavior.Cascade);

            // Transactions
            builder
                .HasMany(c => c.Transactions)
                .WithOne(t => t.Customer)
                .HasForeignKey(t => t.CustomerID)
                .OnDelete(DeleteBehavior.Cascade);

            // Loans
            builder
                .HasMany(c => c.Loans)
                .WithOne(l => l.Customer)
                .HasForeignKey(l => l.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            // Accounts
            builder
                .HasMany(c => c.Accounts)
                .WithOne(a => a.Customer)
                .HasForeignKey(a => a.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);


            // SupportTickets
            builder
                .HasMany(c => c.SupportTickets)
                .WithOne(s => s.Customer)
                .HasForeignKey(s => s.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            // Reservations
            builder
                .HasMany(c => c.Reservations)
                .WithOne(r => r.Customer)
                .HasForeignKey(r => r.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            // FinancialDocuments
            builder
                .HasMany(c => c.FinancialDocument)
                .WithOne(f => f.Customer)
                .HasForeignKey(f => f.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
