using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BankingSystem.DAL.Models;

namespace BankingSystem.DAL.Data.Configurations
{
    public class SavingsConfiguration : IEntityTypeConfiguration<Savings>
    {
        public void Configure(EntityTypeBuilder<Savings> builder)
        {

            builder.Property(s => s.Currency)
                   .IsRequired()
                   .HasMaxLength(10); 

            builder.Property(s => s.Balance)
                   .IsRequired()
                .HasColumnType("decimal(18,4)");

            builder.Property(S => S.IsDeleted)
                .HasDefaultValue(false);

            builder.HasQueryFilter(P => !P.IsDeleted);

        }
    }
}
