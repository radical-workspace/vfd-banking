using BankingSystem.BLL.Interfaces;
using BankingSystem.DAL.Models;
using BankingSystem.PL.ViewModels.Teller;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BankingSystem.PL.Controllers.AppTeller
{
    public class HandleTicketController : Controller
    {
        private readonly IGenericRepository<SupportTicket> _genericRepositoryTicket;
        private readonly ISearchPaginationRepo<SupportTicket> _searchPaginationTicket;


        public HandleTicketController(IGenericRepository<SupportTicket> genericRepositoryTicket, ISearchPaginationRepo<SupportTicket> searchPaginationTicket)
        {
            _genericRepositoryTicket = genericRepositoryTicket;
            _searchPaginationTicket = searchPaginationTicket;
        }



        // GET: HandleTicketController
        public ActionResult Index(string? filter, string? inBranch, int pageNumber = 1)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var tickets = _genericRepositoryTicket.GetAll();


            if (inBranch == "on")
                tickets = _genericRepositoryTicket.GetAll(userId, 2);


            if (filter != null)
                tickets = tickets.Where(t => t.Status.ToString() == filter);


            ViewBag.InBranch = inBranch;
            ViewBag.Filter = filter;

            return View(tickets);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(SupportTicket ticket)
        {
            try
            {
                _genericRepositoryTicket.Update(ticket);
                return RedirectToAction("Index");
            }
            catch
            {
                return NotFound();
            }
        }


        [HttpGet]
        public IActionResult Search(string search)
        {
            var results = _searchPaginationTicket.Search(search);

            ViewBag.Search = search;

            return View(nameof(Index), results);
        }

    }
}
