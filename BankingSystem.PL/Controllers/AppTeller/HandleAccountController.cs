using BankingSystem.BLL.Interfaces;
using BankingSystem.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BankingSystem.PL.Controllers.AppTeller
{
    public class HandleAccountController : Controller
    {
        private readonly IGenericRepository<Account> _genericRepository;
        private readonly IGenericRepository<Customer> _genericRepository2;

        public HandleAccountController(IGenericRepository<Account> genericRepository, IGenericRepository<Customer> genericRepository2)
        {
            _genericRepository = genericRepository;
            _genericRepository2 = genericRepository2;
        }


        // GET: HandleAccountController
        public IActionResult Index()
        {
            return View(_genericRepository.GetAll(User.FindFirst(ClaimTypes.NameIdentifier)?.Value));
        }

        // GET: HandleAccountController/Details/5
        public IActionResult Details(int id)
        {
            return View(_genericRepository.Get(id));
        }

        // GET: HandleAccountController/Create
        public IActionResult Create(string customerId)
        {
            ViewBag.Customer = _genericRepository2.GetAll(customerId).FirstOrDefault();
            return View();
        }

        // POST: HandleAccountController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Account account, string customerId)
        {
            ViewBag.Customer = _genericRepository2.GetAll(customerId).FirstOrDefault();

            if (ModelState.IsValid)
            {
                try
                {
                    _genericRepository.Add(account);
                    return RedirectToAction("index");
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
        public IActionResult Edit(int id)
        {
            return View(_genericRepository.Get(id));
        }

        // POST: HandleAccountController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Account account)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _genericRepository.Update(account);
                    return RedirectToAction("index");
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
        public IActionResult Delete(int id)
        {
            try
            {
                var account = _genericRepository.Get(id);
                if (account == null)
                    return NotFound(); 

                _genericRepository.Delete(account);
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

    }
}
