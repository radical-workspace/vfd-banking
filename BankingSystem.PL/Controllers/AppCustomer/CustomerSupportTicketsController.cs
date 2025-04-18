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
        [HttpPost, HttpGet]
        public IActionResult Details(string id, SupportTicketStatus SelectedStatus = SupportTicketStatus.Pending)
        {
            var customer = _UnitOfWork.Repository<Customer>()
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
            ViewBag.statusList = new SelectList(Enum.GetValues(typeof(SupportTicketStatus)), SelectedStatus);
            return View(SupportTicketModel);
        }
        [HttpGet]
        public IActionResult ApplyTicket(string id)
        {
            var customer = _UnitOfWork.Repository<Customer>()
                            .GetSingleIncluding(c => c.Id == id, c => c.Accounts);

            if (customer != null)
            {
                var accountSelectList = customer.Accounts
                    .Select(a => new SelectListItem
                    {
                        Value = a.Id.ToString(), // assuming Id is the PK and is int/long
                        Text = $"Account No: {a.Number} - {a.AccountType} - Balance: {a.Balance:C}"
                    }).ToList();

                var SupportTicketModel = new CustomerSupportTicket
                {
                    CustomerId = customer.Id,
                    Accounts = accountSelectList,
                    Date = DateTime.Now,
                    Status = SupportTicketStatus.Pending,
                    Type = SupportTicketType.Other
                };
                return View(SupportTicketModel);
            }
            else
            {
                return NotFound($"No Customer Exist for id : {id}");
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ApplyTicket(CustomerSupportTicket model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var supportTicket = new SupportTicket
                {
                    Title = model.Title,
                    Description = model.Description,
                    Date = DateTime.Now,
                    Status = SupportTicketStatus.Pending,
                    Type = model.Type,
                    CustomerId = model.CustomerId,
                    AccountId = model.SelectedAccountId,
                    Response = null,
                    TellerId = null
                };

                _UnitOfWork.Repository<SupportTicket>().Add(supportTicket);
                _UnitOfWork.Complete();

                TempData["SuccessMessage"] = "Your support ticket has been submitted successfully. We'll respond as soon as possible.";

                return RedirectToAction("ThanksTicket", new { id = supportTicket.Id });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while submitting your ticket. Please try again.");
                return View(model);
            }
        }
        public IActionResult ThanksTicket(int id)
        {
            var ticket = _UnitOfWork.Repository<SupportTicket>().Get(id);

            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }



    }
}
