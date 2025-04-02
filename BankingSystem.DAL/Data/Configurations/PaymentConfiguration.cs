using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BankingSystem.DAL.Models;

namespace BankingSystem.DAL.Configurations
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            // Table name
            builder.ToTable("Payments");

            // Primary key
            builder.HasKey(p => p.Id);

            // Properties
            builder.Property(p => p.Amount)
                   .IsRequired()
                   .HasColumnType("decimal(18,4)");

            builder.Property(p => p.PaymentDate)
                   .IsRequired()
                   .HasColumnType("date");

            // Relationships
            builder.HasOne(p => p.Loan)
                   .WithMany(l => l.Payments)
                   .HasForeignKey(p => p.LoanId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
