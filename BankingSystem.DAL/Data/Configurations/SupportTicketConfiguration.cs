using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BankingSystem.DAL.Models;

namespace BankingSystem.DAL.Configurations
{
    public class SupportTicketConfiguration : IEntityTypeConfiguration<SupportTicket>
    {
        public void Configure(EntityTypeBuilder<SupportTicket> builder)
        {
         
           

            // Properties
            builder.Property(st => st.Title)
                   .IsRequired()
                   .HasMaxLength(100); // Assuming a max length for the title

            builder.Property(st => st.Description)
                   .IsRequired()
                   .HasMaxLength(1000); // Assuming a max length for the description


            builder.Property(st => st.Date)
                   .HasDefaultValueSql("GETDATE()");

            builder.Property(st => st.Status)
                  .HasConversion<string>();

            builder.Property(st => st.Type)
                   .HasConversion<string>();

            builder.Property(st => st.Response)
                   .HasMaxLength(1000);

            builder.Property(st => st.IsDeleted)
                  .HasDefaultValue(false);

            builder.HasQueryFilter(P => !P.IsDeleted);


        }
    }
}
