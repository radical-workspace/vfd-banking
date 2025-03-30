using BankingSystem.DAL.Data;
using BankingSystem.DAL.User;
using Microsoft.AspNetCore.Identity;

namespace BankingSystem.PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Add Identity with custom user and roles
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<BankingSystemContext>()
                .AddDefaultTokenProviders();

            // Add authorization policies for each role
            builder.Services.AddAuthorizationBuilder()
                            .AddPolicy("CustomerOnly", policy => policy.RequireRole("Customer"))
                            .AddPolicy("TellerOnly", policy => policy.RequireRole("Teller"))
                            .AddPolicy("ManagerOnly", policy => policy.RequireRole("Manager"));

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

            app.Run();
        }
    }
}
