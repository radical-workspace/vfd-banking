using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BankingSystem.DAL.Models;

namespace BankingSystem.DAL.Configurations
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
          

        

            // Properties
            builder.Property(p => p.Amount)
                   .IsRequired()
                   .HasPrecision(18,4);

            builder.Property(p => p.PaymentDate)
                   .IsRequired()
                   .HasDefaultValueSql("GETDATE()")
                   .HasColumnType("date");

            builder.Property(P => P.IsDeleted)
                  .HasDefaultValue(false);

            builder.HasQueryFilter(P => !P.IsDeleted);

        }
    }
}
