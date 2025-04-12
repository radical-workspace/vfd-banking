using BankingSystem.BLL.Interfaces;
using BankingSystem.DAL.Models;
using BankingSystem.PL.ViewModels.Customer;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.PL.Controllers.AppCustomer
{
    public class CustomerHomeController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;

        public CustomerHomeController(IUnitOfWork UnitOfWork)
        {
            _UnitOfWork = UnitOfWork;
        }

        [HttpGet]
        public IActionResult HomePage(string id)
        {
            var customer = _UnitOfWork.Repository<Customer>()
                           .GetSingleIncluding(c => c.Id == id, c => c.Accounts, /*c => c.Card,*/ c => c.Loans);
            if (customer != null)
            {
                var customerDetailsVM = new CustomerDetailsVM()
                {
                    Id = customer.Id,
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    Email = customer.Email,
                    PhoneNumber = customer.PhoneNumber,
                    Address = customer.Address,
                    JoinDate = customer.JoinDate,
                    BirthDate = customer.BirthDate,
                    TotalBalance = customer.Accounts?.Sum(a => a.Balance),
                    Accounts = customer.Accounts,
                    AccountsCount = customer.Accounts?.Count() ?? 0,
                    //Cards = customer.Cards,
                    //CardsCount = customer.Cards?.Count() ?? 0,
                    //DebitCardsCount = customer.Cards?.Count(c => c.CardType == TypeOfCard.Debit) ?? 0,
                    //CreditCardsCount = customer.Cards?.Count(c => c.CardType == TypeOfCard.Credit) ??0,
                    Loans = customer.Loans,
                    LoansCount = customer.Loans?.Count() ?? 0,
                    CertificatCount = customer.Accounts.Sum(acc => acc.Certificates.Count())
                };
                return View(customerDetailsVM);
            }
            else
            {
                return NotFound($"No Customer Exist for id : {id}");
            }

        }
    }
}
