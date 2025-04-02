using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BankingSystem.DAL.Models;

namespace BankingSystem.DAL.Configurations
{
    public class ManagerConfiguration : IEntityTypeConfiguration<Manager>
    {
        public void Configure(EntityTypeBuilder<Manager> builder)
        {
            // Table name
            builder.ToTable("Managers");

            // Primary key
            builder.HasKey(m => m.Id);

            // Properties
            builder.Property(m => m.IsDeleted)
                   .IsRequired();

            // Relationships
            builder.HasOne(m => m.Branch)
                   .WithOne(b => b.Manager)
                   .HasForeignKey<Branch>(b => b.ManagerId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(m => m.Bank)
                   .WithMany(b => b.Managers)
                   .HasForeignKey(m => m.BankId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
