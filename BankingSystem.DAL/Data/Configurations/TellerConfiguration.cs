using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BankingSystem.DAL.Models;

namespace BankingSystem.DAL.Configurations
{
    public class TellerConfiguration : IEntityTypeConfiguration<Teller>
    {
        public void Configure(EntityTypeBuilder<Teller> builder)
        {
            // Table name
            builder.ToTable("Tellers");

            // Primary key
            builder.HasKey(t => t.Id);

            // Properties
            builder.Property(t => t.IsDeleted)
                   .IsRequired();

            // Relationships
            builder.HasOne(t => t.Department)
                   .WithMany(d => d.Tellers)
                   .HasForeignKey(t => t.DeptId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(t => t.Branch)
                   .WithMany(b => b.Tellers)
                   .HasForeignKey(t => t.BranchId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.Loans)
                   .WithOne(l => l.Teller)
                   .HasForeignKey(l => l.TellerId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
