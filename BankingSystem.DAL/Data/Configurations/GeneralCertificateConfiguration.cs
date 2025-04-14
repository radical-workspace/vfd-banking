using BankingSystem.DAL.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace BankingSystem.DAL.Data.Configurations
{
    internal class GeneralCertificateConfiguration : IEntityTypeConfiguration<GeneralCertificate>
    {
        public void Configure(EntityTypeBuilder<GeneralCertificate> builder)
        {


            builder.Property(g => g.Name)
                   .HasMaxLength(100);

            builder.Property(g => g.Duration)
                   .IsRequired();

            builder.Property(g => g.InterestRate)
                   .IsRequired();

            builder.Property(g => g.Amount)
                   .IsRequired();


            builder.HasQueryFilter(P => !P.IsDeleted);

        }
    }
}