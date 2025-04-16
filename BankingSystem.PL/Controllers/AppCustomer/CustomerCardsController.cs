using AutoMapper;
using BankingSystem.BLL.Interfaces;
using BankingSystem.DAL.Models;
using BankingSystem.PL.ViewModels.Customer;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.PL.Controllers.AppCustomer
{
    public class CustomerCardsController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IMapper _mapper;

        public CustomerCardsController(IUnitOfWork UnitOfWork, IMapper mapper)
        {
            _UnitOfWork = UnitOfWork;
            _mapper = mapper;
        }

        public IActionResult Details(string id)
        {
            var customer = _UnitOfWork.Repository<Customer>()
                                  .GetSingleIncluding(c => c.Id == id, c => c.Accounts);

            customer.Accounts= _UnitOfWork.Repository<Account>().GetAllIncluding(Account => Account.Card).ToList();

            List<VisaCard> cards = [];
            foreach (var account in customer.Accounts)
            {

                account.Card = _UnitOfWork.Repository<VisaCard>()
                    .GetSingleIncluding(c => c.AccountId == account.Id);
                cards.Add(account.Card);

            }

            if (customer != null)
            {
                if (cards.Any())
                {
                    var CardsModel = _mapper.Map<List<CustomerCardsViewModel>>(cards);
                    return View(CardsModel);
                }
                else
                {
                    return RedirectToAction("Details", "CustomerProfile", new { id = customer?.Id });
                }
            }
            else
            {
                return NotFound($"No Customer Exist for id : {id}");
            }

        }
    }
}
