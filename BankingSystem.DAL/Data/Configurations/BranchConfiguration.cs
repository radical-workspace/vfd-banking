using BankingSystem.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BankingSystem.DAL.Data.Configurations
{
    public class BranchConfiguration : IEntityTypeConfiguration<Branch>
    {
        public void Configure(EntityTypeBuilder<Branch> builder)
        {
            builder.Property(Br => Br.Name)
                    .HasMaxLength(50)
                    .IsRequired();
            builder.Property(Br => Br.Location)
                    .HasMaxLength(50)
                    .IsRequired();
            builder.Property(Br => Br.IsDeleted)
                    .HasDefaultValue(false);
            //.OnDelete(DeleteBehavior.Cascade);

            builder.HasQueryFilter(P => !P.IsDeleted);

            builder.HasOne(b => b.MyManager)
              .WithOne(m => m.Branch)
              .HasForeignKey<Manager>(m => m.BranchId);
        }


    }


}
