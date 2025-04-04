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
    public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            //SSN
            builder.Property(U => U.SSN)
                   .HasMaxLength(13)
                   .IsRequired();
            //FirstName
            builder.Property(U => U.FirstName)
                   .HasMaxLength(10)
                   .IsRequired();

            //LastName
            builder.Property(U => U.LastName)
                   .HasMaxLength(10)
                   .IsRequired();

            //Address
            builder.Property(U => U.Address)
                   .HasMaxLength(50)
                   .IsRequired();



            //JoinDate
            builder.Property(C => C.JoinDate)
                   .HasDefaultValueSql("GETDATE()")
                  .HasColumnType("date");
           
            //BirthDate
            builder.Property(U => U.BirthDate)
                  .HasColumnType("date");

            builder.Property(U => U.IsDeleted)
                        .HasDefaultValue(false);

            builder.HasDiscriminator<string>("Discriminator")
                    .HasValue<Manager>("Manager")
                    .HasValue<Teller>("Teller")
                    .HasValue<Customer>("Customer");

            builder.HasQueryFilter(P => !P.IsDeleted);

        }
    }
}
