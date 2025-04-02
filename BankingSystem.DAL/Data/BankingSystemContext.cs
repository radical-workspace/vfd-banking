using BankingSystem.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.DAL.Data
{
    public class BankingSystemContext:DbContext
    {
        public BankingSystemContext(DbContextOptions<BankingSystemContext> options) :base(options) 
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            //base.OnModelCreating(modelBuilder);
        }
        public DbSet <Account> Accounts { get; set; }
        public DbSet <Bank> Banks { get; set; }
        public DbSet <Branch> Branches { get; set; }
        public DbSet <Card> Cards { get; set; }
        public DbSet <Department> Departments{ get; set; }
        public DbSet <Loan> Loans{ get; set; }
        public DbSet <Transaction> Transactionns{ get; set; }
        public DbSet<User> Users { get; set; }
    }
}
