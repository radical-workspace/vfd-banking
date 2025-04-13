using BankingSystem.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.DAL.Data
{
    public class BankingSystemContext(DbContextOptions<BankingSystemContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
           
            modelBuilder.Entity<Customer>().ToTable("Customers");
            modelBuilder.Entity<Admin>().ToTable("Admins");
            modelBuilder.Entity<Manager>().ToTable("Managers");
            modelBuilder.Entity<Teller>().ToTable("Tellers");
            modelBuilder.Entity<ApplicationUser>().ToTable("AspNetUsers");

       

        }
        public DbSet <Account> Accounts { get; set; }
        public DbSet <Bank> Banks { get; set; }
        public DbSet <Branch> Branches { get; set; }
        public DbSet <VisaCard> Cards { get; set; }
        public DbSet<Certificate> Certificates { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Loan> Loans { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Teller> Tellers { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Savings> Savings { get; set; }
        public DbSet<SupportTicket> SupportTickets { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

    }
}
