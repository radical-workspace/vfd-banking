using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BankingSystem.DAL.Models;

namespace BankingSystem.DAL.Configurations
{
    public class SupportTicketConfiguration : IEntityTypeConfiguration<SupportTicket>
    {
        public void Configure(EntityTypeBuilder<SupportTicket> builder)
        {
            // Table name
            builder.ToTable("SupportTickets");

            // Primary key
            builder.HasKey(st => st.Id);

            // Properties
            builder.Property(st => st.Title)
                   .IsRequired()
                   .HasMaxLength(100); // Assuming a max length for the title

            builder.Property(st => st.Description)
                   .IsRequired()
                   .HasMaxLength(1000); // Assuming a max length for the description

            builder.Property(st => st.IsDeleted)
                   .IsRequired();

            builder.Property(st => st.Date)
                   .IsRequired();

            builder.Property(st => st.Status)
                   .IsRequired();

            builder.Property(st => st.Type)
                   .IsRequired();

            builder.Property(st => st.Response)
                   .HasMaxLength(1000); // Assuming a max length for the response

            // Relationships
            builder.HasOne(st => st.Customer)
                   .WithMany(c => c.SupportTickets)
                   .HasForeignKey(st => st.CustomerId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
