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

            builder.Property(a => a.AccountStatus)
                    .HasConversion<string>();


            builder.Property(a => a.Balance)
                .HasDefaultValue(0.0)
                .HasColumnType("decimal(18,4)");


            builder.Property(a => a.IsDeleted)
            .HasDefaultValue(false);

            //builder.HasMany(a => a.Card)
            //   .WithOne(c => c.Account);

            builder.HasQueryFilter(P => !P.IsDeleted);


            builder
            .HasOne(a => a.Customer)
            .WithMany(c => c.Accounts)
            .HasForeignKey(a => a.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

            
            // Transactions
            builder
                .HasMany(a => a.AccountTransactions)
                .WithOne(t => t.Account)
                .HasForeignKey(t => t.AccountId)
                .OnDelete(DeleteBehavior.Cascade);

            // Certificates
            builder
                .HasMany(a => a.Certificates)
                .WithOne(c => c.Account)
                .HasForeignKey(c => c.AccountId)
                .OnDelete(DeleteBehavior.Cascade);

            // Loans
            builder
                .HasMany(a => a.Loans)
                .WithOne(l => l.Account)
                .HasForeignKey(l => l.AccountId)
                .OnDelete(DeleteBehavior.Cascade);

            // SupportTickets
            builder
                .HasMany(a => a.SupportTickets)
                .WithOne(s => s.Account)
                .HasForeignKey(s => s.AccountId)
                .OnDelete(DeleteBehavior.Cascade);

            // VisaCard - assuming one-to-one relationship
            builder
                .HasOne(a => a.Card)
                .WithOne(c => c.Account)
                .HasForeignKey<VisaCard>(c => c.AccountId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
