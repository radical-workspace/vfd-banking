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
            var customer = _UnitOfWork.Repository<MyCustomer>()
                                  .GetSingleIncluding(c => c.Id == id, c => c.Cards);

            if (customer != null)
            {
                    var CardsModel = _mapper.Map<List<CustomerCardsViewModel>>(customer.Cards ?? new List<Card>());
                    return View(CardsModel);
            }
            else
            {
                return NotFound($"No Customer Exist for id : {id}");
            }

        }
    }
}
