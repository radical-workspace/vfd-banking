using BankingSystem.DAL.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.DAL.Data.Configurations
{
    public class FinancialDocumentConfiguration : IEntityTypeConfiguration<FinancialDocument>
    {
        public void Configure(EntityTypeBuilder<FinancialDocument> builder)
        {

            builder.Property(fd => fd.DocumentType)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(fd => fd.Description)
                .HasMaxLength(1000);

            builder.Property(fd => fd.FileName)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(fd => fd.ContentType)
                .IsRequired()
                .HasMaxLength(100);

            // Foreign key without index creation
            //builder.HasOne(fd => fd.Customer)
            //    .WithMany(c => c.FinancialDocuments) //if you have a navigation in MyCustomer
            //    .HasForeignKey(fd => fd.CustomerId)
            //    .OnDelete(DeleteBehavior.Cascade); // or your desired behavior
                                                   //.HasConstraintName("FK_FinancialDocument_Customer");

            //// Prevent automatic index on CustomerId
            //builder.HasIndex(fd => fd.CustomerId).IsUnique(false).IsDescending(false).IsClustered(false);
            //builder.Metadata.RemoveIndex(builder.Metadata.FindIndex(new[] { builder.Property(p => p.CustomerId).Metadata }));

            builder.HasQueryFilter(P => !P.IsDeleted);

        }
    }
}
