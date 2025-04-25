using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BankingSystem.DAL.Models;

namespace BankingSystem.DAL.Configurations
{
    public class TellerConfiguration : IEntityTypeConfiguration<Teller>
    {
        public void Configure(EntityTypeBuilder<Teller> builder)
        {
            builder.Property(t => t.Salary)
                .IsRequired()
                .HasColumnType("decimal(18,4)");
        }
    }
}
