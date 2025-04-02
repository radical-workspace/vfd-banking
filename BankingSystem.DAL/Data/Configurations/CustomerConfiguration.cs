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

      
         

           
        }
    }
}
