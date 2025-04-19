using BankingSystem.DAL.Data;
using BankingSystem.BLL.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using BankingSystem.DAL.Models;
using BankingSystem.BLL.Repositories;
using BankingSystem.DAL.Data.Configurations;
using BankingSystem.BLL;
using BankingSystem.BLL.Interfaces;
using BankingSystem.PL.Helpers;

namespace BankingSystem.PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Configure Services
            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Configure Entity Framework and Identity
            builder.Configuration.AddEnvironmentVariables();
            var DevelopmentconnectionString = builder.Configuration.GetConnectionString("MVCProjectDB");
           
           
            builder.Services.AddDbContext<BankingSystemContext>(options =>
                                                                options.UseSqlServer(DevelopmentconnectionString)
                                                                       .AddInterceptors(new SoftDeleteInterceptor()));


            builder.Services.AddApplicationServices();
            builder.Services.AddAuthentication().AddGoogle(op =>
            {
                op.ClientId = builder.Configuration["Authentication:Google:ClientId"];
                op.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
            });




            #endregion


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            // Ensure roles are created
            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                SeedRoles(roleManager, userManager).Wait();
            }

            app.Run();
        }

        private static async Task SeedRoles(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            string[] roleNames = { "Teller", "Customer", "Manager" ,"Admin"};
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }


            // Create a default admin user
            //var adminUser = new ApplicationUser
            //{
            //    UserName = "admin@admin.com",
            //    Email = "admin@admin.com",
            //    FirstName = "Admin",
            //    LastName = "User",
            //    SSN = 123456789,
            //    Address = "Admin Address",
            //    JoinDate = DateTime.UtcNow,
            //    BirthDate = DateTime.UtcNow.AddYears(-30),
            //    IsDeleted = false
            //};

            //string adminPassword = "Admin@123";
            //var user = await userManager.FindByEmailAsync(adminUser.Email);

            //if (user == null)
            //{
            //    var createAdminUser = await userManager.CreateAsync(adminUser, adminPassword);
            //    if (createAdminUser.Succeeded)
            //    {
            //        await userManager.AddToRoleAsync(adminUser, "Admin");
            //    }
            //}
        }
    }
}
