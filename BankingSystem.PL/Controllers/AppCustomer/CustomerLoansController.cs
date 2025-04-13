using AutoMapper;
using BankingSystem.BLL.Interfaces;
using BankingSystem.DAL.Models;
using BankingSystem.PL.ViewModels.Customer;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.PL.Controllers.AppCustomer
{
    public class CustomerLoansController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IMapper _mapper;

        public CustomerLoansController(IUnitOfWork UnitOfWork, IMapper mapper)
        {
            _UnitOfWork = UnitOfWork;
            _mapper = mapper;
        }

        public IActionResult Details(string id)
        {
            var customer = _UnitOfWork.Repository<MyCustomer>()
                                  .GetSingleIncluding(c => c.Id == id, c => c.Loans);

            if (customer != null)
            {
                if (customer.Loans?.Any() ?? false)
                {
                    var LoanModel = _mapper.Map<List<CustomerLoansViewModel>>(customer.Loans);
                    return View(LoanModel);
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
