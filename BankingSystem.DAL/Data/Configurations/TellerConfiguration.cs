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

        

            


       

        }
    }
}
