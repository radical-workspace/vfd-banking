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
    public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.Property(D => D.Id)
                    .UseIdentityColumn(200, 1);

            builder.Property(D => D.Name)
                    .HasMaxLength(30)
                    .IsRequired();
            builder.Property(D => D.IsDeleted)
                    .HasDefaultValue(false);
            builder.HasQueryFilter(P => !P.IsDeleted);

        }
    }
}
