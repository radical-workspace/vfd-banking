using AutoMapper;
using BankingSystem.BLL.Interfaces;
using BankingSystem.DAL.Models;
using BankingSystem.PL.ViewModels.Customer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BankingSystem.PL.Controllers.AppCustomer
{
    public class CustomerTransactionsController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IMapper _mapper;

        public CustomerTransactionsController(IUnitOfWork UnitOfWork, IMapper mapper)
        {
            _UnitOfWork = UnitOfWork;
            _mapper = mapper;
        }

        [HttpPost, HttpGet]
        public IActionResult Details(string id, TransactionStatus SelectedStatus = TransactionStatus.Accepted)
        {
            var customer = _UnitOfWork.Repository<Customer>()
              .GetSingleDeepIncluding(
                  c => c.Id == id,
                  q => q.Include(c => c.Transactions).ThenInclude(t => t.Payment)
              );

            if (customer == null)
            {
                return NotFound($"no customer exist for id : {id}");

            }

            var transactions = customer.Transactions?
                .Where(s => s.Status == SelectedStatus).ToList() ?? new List<Transaction>();

            var TransactionModel = new CustomerTransactionVM
            {
                Transactions = _mapper.Map<List<TransactionMinimal>>(transactions),
                SelectedStatus = SelectedStatus,
                Id = customer.Id
            };
            ViewBag.statusList = new SelectList(Enum.GetValues(typeof(TransactionStatus)), SelectedStatus);
            return View(TransactionModel);
        }
    }
}
