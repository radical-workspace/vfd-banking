using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BankingSystem.DAL.Models;

namespace BankingSystem.DAL.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            // Table name
            builder.ToTable("Customers");

            // Primary key
            builder.HasKey(c => c.Id);

            // Properties
            builder.Property(c => c.IsDeleted)
                   .IsRequired();

            // Relationships
            builder.HasMany(c => c.Transactionns)
                   .WithOne(t => t.Customer)
                   .HasForeignKey(t => t.CustomerId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.Loans)
                   .WithOne(l => l.Customer)
                   .HasForeignKey(l => l.CustomerId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.Accounts)
                   .WithOne(a => a.Customer)
                   .HasForeignKey(a => a.CustomerId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(c => c.Card)
                   .WithOne()
                   .HasForeignKey<Customer>(c => c.CardId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
