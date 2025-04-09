using BankingSystem.BLL.Interfaces;
using BankingSystem.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.PL.Controllers.AppTeller
{
    public class HandleAccountController : Controller
    {
        private readonly IGenericRepository<Account> _genericRepository;

        public HandleAccountController(IGenericRepository<Account> genericRepository)
        {
            _genericRepository = genericRepository;
        }


        // GET: HandleAccountController
        public IActionResult Index()
        {
            return View(_genericRepository.GetAll());
        }

        // GET: HandleAccountController/Details/5
        public IActionResult Details(int id)
        {
            return View(_genericRepository.Get(id));
        }

        // GET: HandleAccountController/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: HandleAccountController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: HandleAccountController/Edit/5
        public IActionResult Edit(int id)
        {
            return View();
        }

        // POST: HandleAccountController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
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
