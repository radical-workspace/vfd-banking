using BankingSystem.DAL.User;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.DAL.Data
{
    public class BankingSystemContext(DbContextOptions<BankingSystemContext> options) : DbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Seed initial roles
            builder.Entity<ApplicationRole>().HasData(
                new ApplicationRole { Id = 1, Title= "Admin", Description = "ADMIN" },
                new ApplicationRole { Id = 2, Title = "User", Description = "USER" }
            );
        }
        //    public DbSet<Models.Customer> Customers { get; set; }
        //    public DbSet<Models.Transaction> Transactions { get; set; }
        //    public DbSet<Models.Account> Accounts { get; set; }
        //    public DbSet<Models.Branch> Branches { get; set; }
        //    public DbSet<Models.Employee> Employees { get; set; }
        //    public DbSet<Models.Manager> Managers { get; set; }
        //    public DbSet<Models.Teller> Tellers { get; set; }
        //    public DbSet<Models.User> Users { get; set; }
        //    public DbSet<Models.UserRole> UserRoles { get; set; }
        //    public DbSet<Models.UserClaim> UserClaims { get; set; }
        //    public DbSet<Models.UserLogin> UserLogins { get; set; }
        //    public DbSet<Models.UserToken> UserTokens { get; set; }
        //    public DbSet<Models.Role> Roles { get; set; }
        //    public DbSet<Models.RoleClaim> RoleClaims { get; set; }
        //    public DbSet<Models.CustomerProfile> CustomerProfiles { get; set; }
        //    public DbSet<Models.TellerProfile> TellerProfiles { get; set; }
        //    public DbSet<Models.ManagerProfile> ManagerProfiles { get; set; }
        //    public DbSet<Models.ApplicationRole> ApplicationRoles { get; set; }
        //    public DbSet<Models.ApplicationUser> ApplicationUsers { get; set; }
        //    protected override void OnModelCreating(ModelBuilder modelBuilder)
        //    {
        //        modelBuilder.Entity<Models.Customer>().ToTable("Customer");
        //        modelBuilder.Entity<Models.Transaction>().ToTable("Transaction");
        //        modelBuilder.Entity<Models.Account>().ToTable("Account");
        //        modelBuilder.Entity<Models.Branch>().ToTable("Branch");
        //        modelBuilder.Entity<Models.Employee>().ToTable("Employee");
        //        modelBuilder.Entity<Models.Manager>().ToTable("Manager");
        //        modelBuilder.Entity<Models.Teller>().ToTable("Teller");
        //        modelBuilder.Entity<Models.User>().ToTable("User");
        //        modelBuilder.Entity<Models.UserRole>().ToTable("UserRole");
        //        modelBuilder.Entity<Models.UserClaim>().ToTable("UserClaim");
        //        modelBuilder.Entity<Models.UserLogin>().ToTable("UserLogin");
        //        modelBuilder.Entity<Models.UserToken>().ToTable("UserToken");
        //        modelBuilder.Entity<Models.Role>().ToTable("Role");
        //        modelBuilder.Entity < Models.RoleClaim
    }
}
