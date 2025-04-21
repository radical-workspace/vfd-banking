using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BankingSystem.DAL.Models;

namespace BankingSystem.DAL.Configurations
{
    public class ManagerConfiguration : IEntityTypeConfiguration<Manager>
    {
        public void Configure(EntityTypeBuilder<Manager> builder)
        {
            builder.HasOne(m => m.Branch)
               .WithOne(b => b.MyManager)
               .HasForeignKey<Manager>(m => m.BranchId);

            builder.Property(m => m.Salary)
            .IsRequired()
             .HasColumnType("decimal(18,4)");
            builder.HasOne(m => m.Branch)
           .WithOne(b => b.MyManager)
           .HasForeignKey<Manager>(m => m.BranchId)
           .OnDelete(DeleteBehavior.SetNull); // or use Cascade/DeleteRestrict if you prefer

            builder.Ignore(m => m.FullName); // Already marked as [NotMapped], but just in case
        }
    }
}
