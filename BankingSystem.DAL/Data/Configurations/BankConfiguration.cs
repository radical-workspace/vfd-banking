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
    public class BankConfiguration : IEntityTypeConfiguration<Bank>
    {
        public void Configure(EntityTypeBuilder<Bank> builder)
        {
            builder.Property(B => B.Name)
                   .HasMaxLength(30)
                   .IsRequired();

            builder.Property(B => B.CentralAddress)
               .HasMaxLength(30)
               .IsRequired();
        }
    }
}
