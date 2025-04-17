using BankingSystem.BLL.Interfaces;
using BankingSystem.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BankingSystem.PL.Controllers.AppTeller
{
    public class HandleAccountController : Controller
    {
        private readonly IGenericRepository<Account> _genericRepositoryAcc;
        private readonly IGenericRepository<Customer> _genericRepositoryCust;
        private readonly ISearchPaginationRepo<Account> _searchPaginationRepo;

        public HandleAccountController(IGenericRepository<Account> genericRepository, IGenericRepository<Customer> genericRepository2,
            ISearchPaginationRepo<Account> searchPaginationRepo)
        {
            _genericRepositoryAcc = genericRepository;
            _genericRepositoryCust = genericRepository2;
            _searchPaginationRepo = searchPaginationRepo;
        }


        // GET: HandleAccountController
        public IActionResult Index(string? filter, int pageNumber = 1)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var accounts = _searchPaginationRepo.GetAllByPagination(userId, filter, out int totalRecords, out int totalPages, pageNumber);


            ViewBag.CurrentPage = pageNumber;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalRecords = totalRecords;
            ViewBag.Filter = filter;

            return View(accounts);
        }


        [HttpGet]
        public IActionResult Search(string search)
        {
            var tellerID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var results = _searchPaginationRepo.Search(search, tellerID);

            ViewBag.Search = search;
            ViewBag.TotalRecords = results.Count();

            return View("Index", results);
        }



        // GET: HandleAccountController/Details/5
        public IActionResult Details(int id)
        {
            return View(_genericRepositoryAcc.Get(id));
        }

        // GET: HandleAccountController/Create
        public IActionResult Create(string customerId)
        {
            ViewBag.Customer = _genericRepositoryCust.GetAll(customerId).FirstOrDefault();
            return View();
        }

        // POST: HandleAccountController/Create
        [HttpPost]
       
        public IActionResult Create(Account account, string customerId)
        {
            ViewBag.Customer = _genericRepositoryCust.GetAll(customerId).FirstOrDefault();

            if (ModelState.IsValid)
            {
                try
                {
                    _genericRepositoryAcc.Add(account);
                    return RedirectToAction("ShowAccounts", "HandleCustomer", new { id = customerId });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(account);
                }
            }
            return View(account);
        }

        // GET: HandleAccountController/Edit/5
        public IActionResult Edit(int id, string returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View(_genericRepositoryAcc.Get(id));
        }

        // POST: HandleAccountController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Account account, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _genericRepositoryAcc.Update(account);

                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);
                    
                    return RedirectToAction("Index");
                }
                catch
                {
                    return View(account);
                }
            }
            return View(account);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, string? returnUrl = null)
        {
            try
            {
                var account = _genericRepositoryAcc.Get(id);
                if (account == null)
                    return NotFound(); 

                _genericRepositoryAcc.Delete(account);
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }

            if (returnUrl != null)
                return Redirect(returnUrl);

            return RedirectToAction(nameof(Index));
        }

    }
}
