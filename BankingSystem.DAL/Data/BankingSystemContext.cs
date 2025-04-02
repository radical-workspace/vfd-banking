using BankingSystem.DAL.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.DAL.Data
{
    public class BankingSystemContext(DbContextOptions<BankingSystemContext> options) : IdentityDbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            //base.OnModelCreating(modelBuilder);
        }
        public DbSet <Account> Accounts { get; set; }
        public DbSet <Bank> Banks { get; set; }
        public DbSet <Branch> Branches { get; set; }
        public DbSet <Card> Cards { get; set; }
        public DbSet<Certificate> Certificates { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Loan> Loans { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<Savings> Savings { get; set; }
        public DbSet<SupportTicket> SupportTickets { get; set; }
        public DbSet<MyTransaction> Transactions { get; set; }
        
        //public DbSet<Teller> Tellers { get; set; }
        //public DbSet<Customer> Customers { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
