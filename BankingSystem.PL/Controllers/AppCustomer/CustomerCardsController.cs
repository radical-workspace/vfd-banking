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
            var cards = _UnitOfWork.Repository<VisaCard>().GetAllIncluding(c => c.Account)
                                                            .Where(c => c.Account.CustomerId == id).ToList();


            if (cards != null)
            {

                var CardsModel = _mapper.Map<List<CustomerCardsViewModel>>(cards);

                //var CardsModel = _mapper.Map<List<CustomerCardsViewModel>>(customer.Accounts?.Where(acc => acc.Card != null).Select(acc=>acc.Card) ?? new List<VisaCard>());
                return View(CardsModel);
            }
            else
            {
                return NotFound($"No Customer Exist for id : {id}");
            }

        }
    }
}
