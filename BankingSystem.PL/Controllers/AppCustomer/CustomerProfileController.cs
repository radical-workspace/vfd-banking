using AutoMapper;
using BankingSystem.BLL.Interfaces;
using BankingSystem.DAL.Models;
using BankingSystem.PL.ViewModels.Customer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BankingSystem.PL.Controllers.AppCustomer
{
    public class CustomerProfileController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IMapper mapper;

        public CustomerProfileController(IUnitOfWork UnitOfWork , IMapper _mapper)
        {
            _UnitOfWork = UnitOfWork;
            mapper = _mapper;
        }

        [HttpGet]
        public IActionResult Details(string id)
        {
            var customer = _UnitOfWork.Repository<MyCustomer>()
              .GetSingleDeepIncluding(
                  c => c.Id == id,
                  q => q.Include(c => c.Accounts).ThenInclude(a => a.Certificates),
                  q => q.Include(c => c.Cards),
                  q => q.Include(c => c.Loans),
                  q => q.Include(c => c.Transactions)
              );


            if (customer != null)
            {
                var CustomerProfileModel =mapper.Map<CustomerProfileViewModel>(customer);

                CustomerProfileModel.TotalBalance = customer.Accounts?.Sum(acc => acc.Balance ?? 0) ?? 0;
                CustomerProfileModel.AccountsCount = customer.Accounts?.Count() ?? 0;
                CustomerProfileModel.CardsCount = customer.Cards?.Count() ?? 0;
                CustomerProfileModel.DebitCardsCount = customer.Cards?.Count(c => c.CardType == TypeOfCard.Debit) ?? 0;
                CustomerProfileModel.CreditCardsCount = customer.Cards?.Count(c => c.CardType == TypeOfCard.Credit) ?? 0;
                CustomerProfileModel.LoansCount = customer.Loans?.Count() ?? 0;
                CustomerProfileModel.CertificatesCount = customer.Accounts?.Sum(acc => acc.Certificates?.Count() ?? 0) ?? 0;
            
                return View(CustomerProfileModel);
            }
            else
            {
                return NotFound($"No Customer Exist for id : {id}");
            }

        }

        [HttpGet]
        public IActionResult Edit(string id)
        {
            var customer = _UnitOfWork.Repository<MyCustomer>().GetSingleIncluding(c => c.Id == id);
            if (customer != null)
            {
                var customerViewModel = mapper.Map<CustomerViewModel>(customer);
                return View(customerViewModel);
            }
            else
            {
                return NotFound($"No Customer Exist for id : {id}");
            }
        }

        [HttpPost]
        public IActionResult Edit( CustomerViewModel CustomerVM )
        {
           
            if( CustomerVM != null && ModelState.IsValid)
            {
                var CustomerToUpdate = _UnitOfWork.Repository<MyCustomer>().GetSingleIncluding( c=> c.Id == CustomerVM.Id);
                if (CustomerToUpdate == null)
                {
                    return NotFound($"No Customer Exist for id : {CustomerVM.Id}");
                }
                else
                {
                    // (preserves navigation properties &EF Core tracking)
                    CustomerToUpdate = mapper.Map(CustomerVM, CustomerToUpdate);
                    _UnitOfWork.Repository<MyCustomer>().Update(CustomerToUpdate);
                    _UnitOfWork.Complete();
                    return RedirectToAction("Details", new { id = CustomerToUpdate.Id });
                }
            }
            // else
            return View(CustomerVM);
        }
    }
}
