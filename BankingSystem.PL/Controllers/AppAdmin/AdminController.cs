using BankingSystem.BLL.Interfaces;
using BankingSystem.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.PL.Controllers.AppAdmin
{
    public class AdminController : Controller
    {
        private readonly IGenericRepository<Admin> _genericRepositoryAdmin;
        private readonly ISearchPaginationRepo<Admin> _searchPaginationAdmin;

        public AdminController(IGenericRepository<Admin> genericRepositoryAdmin, ISearchPaginationRepo<Admin> searchPaginationAdmin)
        {
            _genericRepositoryAdmin = genericRepositoryAdmin;
            _searchPaginationAdmin = searchPaginationAdmin;
        }


        // GET: AdminController
        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult Dashboard()
        {
            return View();
        }


        // GET: AdminController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AdminController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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

        // GET: AdminController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AdminController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
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

        // GET: AdminController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AdminController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
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
    }
}
