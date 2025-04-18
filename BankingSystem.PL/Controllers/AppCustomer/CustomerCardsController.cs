using AutoMapper;
using BankingSystem.BLL.Interfaces;
using BankingSystem.DAL.Models;
using BankingSystem.PL.ViewModels.Customer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
                                  .GetSingleDeepIncluding(c => c.Id == id,
                                  q => q.Include(c => c.Accounts).ThenInclude(a => a.Card));

            if (customer != null)
            {

                var CardsModel = _mapper.Map<List<CustomerCardsViewModel>>(customer.Accounts?.Where(acc => acc.Card != null).Select(acc=>acc.Card) ?? new List<VisaCard>());
                    return View(CardsModel);
            }
            else
            {
                return NotFound($"No Customer Exist for id : {id}");
            }

        }
    }
}
