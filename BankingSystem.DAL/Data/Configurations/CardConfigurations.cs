using BankingSystem.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.DAL.Data.Configurations
{
    public class CardConfigurations : IEntityTypeConfiguration<VisaCard>
    {
        public void Configure(EntityTypeBuilder<VisaCard> builder)
        {
            builder.Property(C => C.Number)
                 .HasMaxLength(16)
                 .IsRequired();

            builder.Property(C => C.CVV)
             .HasMaxLength(3)
             .IsRequired();

            builder.Property(C => C.ExpDate)
                    .HasColumnType("date");

            builder.Property(C => C.CreationDate)
                    .HasDefaultValueSql("GETDATE()")
                   .HasColumnType("date");

            builder.Property(c => c.CardType)
                        .HasConversion<string>();

            builder.Property(C => C.IsDeleted)
                .HasDefaultValue(false);

            builder.HasQueryFilter(P => !P.IsDeleted);

        }
    }
}
