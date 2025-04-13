using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BankingSystem.DAL.Models;
using BankingSystem.DAL.Data.Configurations;

namespace BankingSystem.DAL.Configurations
{
    public class CertificateConfiguration : IEntityTypeConfiguration<Certificate>
    {
        public void Configure(EntityTypeBuilder<Certificate> builder)
        {
            builder.HasIndex(c => c.CertificateNumber)
                   .IsUnique();

            builder.Property(c => c.CertificateNumber)
                   .IsRequired();

            builder.Property(c => c.IssueDate)
                   .IsRequired();

            builder.Property(c => c.ExpiryDate)
                   .IsRequired();


            builder.HasQueryFilter(P => !P.IsDeleted);
        }

    }
}

