using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BankingSystem.DAL.Models;
using System.Reflection.Emit;

namespace BankingSystem.DAL.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<MyCustomer>
    {
        public void Configure(EntityTypeBuilder<MyCustomer> builder)
        {

            builder
           .HasOne(c => c.Branch)
           .WithMany(b => b.Customers) // Assuming Branch has many Customers
           .HasForeignKey(c => c.BranchId) // Explicit FK
           .OnDelete(DeleteBehavior.SetNull); // Or Cascade
        }
    }
}
