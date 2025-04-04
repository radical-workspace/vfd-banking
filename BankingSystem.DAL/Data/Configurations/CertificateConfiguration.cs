using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BankingSystem.DAL.Models;

namespace BankingSystem.DAL.Configurations
{
    public class CertificateConfiguration : IEntityTypeConfiguration<Certificate>
    {
        public void Configure(EntityTypeBuilder<Certificate> builder)
        {
            // Table name
            builder.ToTable("Certificates");

            // Primary key
            builder.HasKey(c => c.Id);

            // Properties
            builder.Property(c => c.CertificateNumber)
                   .IsRequired()
                   .HasMaxLength(50); // Assuming a max length for certificate number

            builder.Property(c => c.IssueDate)
                   .IsRequired();

            builder.Property(c => c.ExpiryDate)
                   .IsRequired();

            builder.Property(c => c.Amount)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            builder.Property(c => c.IsDeleted)
                   .IsRequired();

            // Relationships
            builder.HasOne(c => c.Account)
                   .WithMany(a => a.Certificates)
                   .HasForeignKey(c => c.AccountId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasQueryFilter(P => !P.IsDeleted);

        }
    }
}
