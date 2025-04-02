using BankingSystem.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace BankingSystem.DAL.Data.Configurations
{
    public class TransactionnnConfiguration : IEntityTypeConfiguration<Models.Transaction>
    {
        public void Configure(EntityTypeBuilder<Models.Transaction> builder)
        {
            // Ensure Amount is a decimal for financial precision

            builder.Property(T => T.Id)
                    .UseIdentityColumn(1000, 1);

            builder.Property(T => T.Amount)
                .HasColumnType("decimal(18,4)")
                .IsRequired();

            // Set default value for Date
            builder.Property(T => T.Date)
                .HasColumnType("datetime2")
                .HasDefaultValueSql("GETDATE()");

            
            builder.Property(T => T.Status)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(T => T.IsDeleted)
                    .HasDefaultValue(false);
        }
    }
}
