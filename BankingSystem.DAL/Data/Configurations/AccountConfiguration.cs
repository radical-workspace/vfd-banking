using BankingSystem.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.DAL.Data.Configurations
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.Property(a => a.AccountType)
                    .HasConversion<string>();

         
            builder.Property(a => a.Balance)
                .HasDefaultValue(0.0)
                .HasColumnType("decimal(18,4)"); 

            
            builder.Property(a => a.IsDeleted)
            .HasDefaultValue(false);

            builder.HasMany(a => a.Cards)
               .WithOne(c => c.Account);
            builder.HasQueryFilter(P => !P.IsDeleted);

        }
    }
}
