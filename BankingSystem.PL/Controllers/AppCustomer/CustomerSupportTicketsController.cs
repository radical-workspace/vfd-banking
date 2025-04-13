using AutoMapper;
using BankingSystem.BLL.Interfaces;
using BankingSystem.DAL.Models;
using BankingSystem.PL.ViewModels.Customer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BankingSystem.PL.Controllers.AppCustomer
{
    public class CustomerSupportTicketsController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IMapper _mapper;

        public CustomerSupportTicketsController(IUnitOfWork UnitOfWork, IMapper mapper)
        {
            _UnitOfWork = UnitOfWork;
            _mapper = mapper;
        }
        [HttpPost , HttpGet]
        public IActionResult Details(string id , SupportTicketStatus SelectedStatus = SupportTicketStatus.Pending)
        {
            var customer = _UnitOfWork.Repository<MyCustomer>()
                                  .GetSingleIncluding(c => c.Id == id, c => c.SupportTickets);
            if (customer == null)
            {
                return NotFound($"no customer exist for id : {id}");

            }

            var tickets = customer.SupportTickets?
                .Where(s => s.Status == SelectedStatus).ToList() ?? new List<SupportTicket>();

                    var SupportTicketModel = new CustomerSupportTicketsViewModel
                    {
                        Tickets = _mapper.Map<List<CustomerSupportTicket>>(tickets),
                        SelectedStatus = SelectedStatus,
                        Id = customer.Id
                    };
                    ViewBag.statusList = new SelectList(Enum.GetValues(typeof(SupportTicketStatus)) , SelectedStatus);
                    return View(SupportTicketModel);
        }

    }
}
