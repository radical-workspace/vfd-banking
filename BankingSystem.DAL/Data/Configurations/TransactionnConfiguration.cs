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
    public class MyTransactionConfiguration : IEntityTypeConfiguration<Models.MyTransaction>
    {
        public void Configure(EntityTypeBuilder<Models.MyTransaction> builder)
        {
            // Ensure Amount is a decimal for financial precision

            builder.Property(T => T.Id)
                    .UseIdentityColumn(1000, 1);

            builder.Property(T => T.DoneVia)
                .IsRequired()
                .HasMaxLength(50);  

            builder.Property(T => T.PaymentId)
                .IsRequired()
                .HasColumnName("PaymentId"); // FK

            builder.Property(T => T.Status)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(T => T.Type)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(T => T.IsDeleted)
                    .HasDefaultValue(false);

            builder.HasQueryFilter(P => !P.IsDeleted);

        }
    }
}
