using AutoMapper;
using BankingSystem.BLL.Interfaces;
using BankingSystem.DAL.Models;
using BankingSystem.PL.ViewModels.Customer;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.PL.Controllers.AppCustomer
{
    public class CustomerAccountsController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IMapper _mapper;

        public CustomerAccountsController(IUnitOfWork UnitOfWork, IMapper mapper)
        {
            _UnitOfWork = UnitOfWork;
            _mapper = mapper;
        }

        public IActionResult Details(string id)
        {
            var customer = _UnitOfWork.Repository<Customer>()
                                  .GetSingleIncluding(c => c.Id == id, c => c.Accounts);

            if (customer != null && (customer.Accounts?.Any() ?? false))
            {
                var AccountsModel = _mapper.Map<List<CustomerAccountsViewModel>>(customer.Accounts);
                ViewBag.id = customer.Id;
                return View(AccountsModel);
            }
            else
            {
                return NotFound($"No Customer Exist for id : {id}");
            }

        }
    }
}
