using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BankingSystem.DAL.Models;

namespace BankingSystem.DAL.Data.Configurations
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
                   .HasMaxLength(10); 

            builder.Property(s => s.Balance)
                   .IsRequired()
                   .HasPrecision(18, 4);


        }
    }
}
