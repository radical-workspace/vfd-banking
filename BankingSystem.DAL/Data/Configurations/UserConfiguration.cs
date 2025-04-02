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
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            //Id
            builder.Property(U => U.Id)
                   .UseIdentityColumn(1, 1)
                   .IsRequired();

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

            //Phone
            builder.Property(U => U.Phone)
                   .HasMaxLength(11);


            //JoinDate
            builder.Property(C => C.JoinDate)
                   .HasDefaultValueSql("GETDATE()")
                  .HasColumnType("date");
           
            //BirthDate
            builder.Property(U => U.BirthDate)
                  .HasColumnType("date")
                   ;// Ensures only date is stored (without time)

            builder.Property(U => U.IsDeleted)
                        .HasDefaultValue(false);

            builder.HasDiscriminator<string>("Discriminator")
                    .HasValue<Manager>("Manager")
                    .HasValue<Teller>("Teller")
                    .HasValue<Customer>("Customer");
                    

            //builder.HasDiscriminator<string>("UserType")
            // .HasValue<Frontend>("Frontend")
            // .HasValue<Kitchen>("Kitchen");




        }
    }
}
