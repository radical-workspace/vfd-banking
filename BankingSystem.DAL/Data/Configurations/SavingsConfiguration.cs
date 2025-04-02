using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BankingSystem.DAL.Models;

namespace BankingSystem.DAL.Configurations
{
    public class SavingsConfiguration : IEntityTypeConfiguration<Savings>
    {
        public void Configure(EntityTypeBuilder<Savings> builder)
        {
            // Table name
            builder.ToTable("Savings");

            // Primary key
            builder.HasKey(s => s.Id);

            // Properties
            builder.Property(s => s.IsDeleted)
                   .IsRequired();

            builder.Property(s => s.Currency)
                   .IsRequired()
                   .HasMaxLength(3); // Assuming currency code length is 3

            builder.Property(s => s.Balance)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            // Relationships
            builder.HasOne(s => s.Branch)
                   .WithMany(b => b.Savings)
                   .HasForeignKey(s => s.BracketId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
