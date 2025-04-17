using BankingSystem.BogusFakers;
using BankingSystem.DAL.Data;
using BankingSystem.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BankingSystem.PL.Controllers
{
    public class BogusDataController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly BankingSystemContext _context;

        public BogusDataController(UserManager<ApplicationUser> userManager, BankingSystemContext context)
        {
            _userManager = userManager;
            _context = context;
        }


        [HttpGet]
        public IActionResult CreateBogusData()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> CreateAdmins()
        {
            return await CreateFakers(fakers: Faker.GenerateFakeAdmins(5), role: "Admin");
        }


        [HttpPost]
        public async Task<IActionResult> CreateManagers()
        {
            return await CreateFakers(fakers: Faker.GenerateFakeManagers(5), role: "Manager");
        }


        [HttpPost]
        public async Task<IActionResult> CreateTellers()
        {
            return await CreateFakers(fakers: Faker.GenerateFakeTellers(5), role: "Teller");
        }


        [HttpPost]
        public async Task<IActionResult> CreateClients()
        {
            return await CreateFakers(fakers: Faker.GenerateFakeClients(5), role: "Customer");
        }


        [HttpPost]
        public async Task<IActionResult> CreateAccounts()
        {
            var customers = await _context.Customers.ToListAsync();
            var accounts = Faker.GenerateFakeAccounts(customers);

            _context.Accounts.AddRange(accounts);
            await _context.SaveChangesAsync();

            return RedirectToAction("CreateBogusData");
        }


        [HttpPost]
        public async Task<IActionResult> CreateBranches()
        {
            var fakeBranches = Faker.GenerateFakeBranches(5);
            _context.Branches.AddRange(fakeBranches);
            await _context.SaveChangesAsync();

            return RedirectToAction("CreateBogusData");
        }


        [HttpPost]
        public async Task<IActionResult> CreateBanks()
        {
            var fakeBanks = Faker.GenerateFakeBanks(5);
            _context.Banks.AddRange(fakeBanks);
            await _context.SaveChangesAsync();

            return RedirectToAction("CreateBogusData");
        }
        
        
        [HttpPost]
        public async Task<IActionResult> CreateTickets()
        {
            var fakeBanks = Faker.GenerateFakeTickets(5);
            _context.SupportTickets.AddRange(fakeBanks);
            await _context.SaveChangesAsync();

            return RedirectToAction("CreateBogusData");
        }



        private async Task<IActionResult> CreateFakers(dynamic fakers, string role)
        {
            string password = "Test@123";

            foreach (var teller in fakers)
            {
                var result = await _userManager.CreateAsync(teller, password);
                if (result.Succeeded)
                    await _userManager.AddToRoleAsync(teller, role);

                else
                    foreach (var error in result.Errors)
                        ModelState.AddModelError("", error.Description);
            }

            return RedirectToAction("CreateBogusData");
        }

    }
}
