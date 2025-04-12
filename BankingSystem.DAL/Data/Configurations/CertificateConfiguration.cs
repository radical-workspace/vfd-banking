using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BankingSystem.DAL.Models;

namespace BankingSystem.DAL.Configurations
{
    public class CertificateConfiguration : IEntityTypeConfiguration<Certificate>
    {
        public void Configure(EntityTypeBuilder<Certificate> builder)
        {

        

            // Properties
            builder.Property(c => c.CertificateNumber)
                   .IsRequired()
                   .HasMaxLength(50); // Assuming a max length for certificate number

            builder.Property(c => c.IssueDate)
                .HasDefaultValueSql("GETDATE()")
                   .IsRequired();

            builder.Property(c => c.ExpiryDate)
                   .IsRequired();

            //builder.Property(c => c.Amount)
            //       .IsRequired()
            //       .HasColumnType("decimal(18,4)");

            builder.Property(c => c.IsDeleted)
                   .HasDefaultValue(false);

          
            builder.HasQueryFilter(P => !P.IsDeleted);

        }
    }
}
