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
    internal class AssetConfiguration : IEntityTypeConfiguration<Asset>
    {
        public void Configure(EntityTypeBuilder<Asset> builder)
        {
            builder.Property(a => a.EstimatedValue)
                .HasDefaultValue(0.0)
                .HasColumnType("decimal(18,4)");
            builder.Property(a => a.Type)
                .IsRequired()
                .HasMaxLength(100); // Adjust the length as needed
        }
    }
}
