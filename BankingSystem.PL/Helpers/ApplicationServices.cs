using BankingSystem.BLL.Interfaces;
using BankingSystem.BLL.Services;
using BankingSystem.BLL;
using BankingSystem.DAL.Data;
using BankingSystem.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace BankingSystem.PL.Helpers
{
    public static class ApplicationServices
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection Services )
        {
            Services.AddIdentity<ApplicationUser, IdentityRole>(op =>
            {
                op.Password.RequireUppercase = false;

                op.Password.RequiredLength = 4;
                op.Password.RequireNonAlphanumeric = false;
            })
               .AddEntityFrameworkStores<BankingSystemContext>()
               .AddDefaultUI()
               .AddDefaultTokenProviders();

            // Register Unit of Work
            Services.AddScoped<IUnitOfWork, UnitOfWork>();
            Services.AddScoped<IGenericRepository<Account>, MyAccountBL>();
            Services.AddScoped<IGenericRepository<Customer>, MyCustomerBL>();
            Services.AddScoped<IGenericRepository<VisaCard>, MyCardBL>();
            Services.AddScoped<IGenericRepository<SupportTicket>, MyTicketBL>();
            Services.AddScoped<IGenericRepository<Admin>, MyAdminBL>();

            Services.AddScoped<HandleAccountTransferes>();

            Services.AddScoped<ISearchPaginationRepo<Account>, MyAccountBL>();
            Services.AddScoped<ISearchPaginationRepo<Customer>, MyCustomerBL>();
            Services.AddScoped<ISearchPaginationRepo<VisaCard>, MyCardBL>();
            Services.AddScoped<ISearchPaginationRepo<SupportTicket>, MyTicketBL>();
            Services.AddScoped<ISearchPaginationRepo<Admin>, MyAdminBL>();

               Services.AddScoped<FinancialDocumentService>();
               Services.AddAutoMapper(M => M.AddProfile(new MappingProfile()));
            return Services;
        }
    }
}
