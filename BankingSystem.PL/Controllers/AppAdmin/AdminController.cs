using BankingSystem.BLL;
using BankingSystem.BLL.Interfaces;
using BankingSystem.DAL.Models;
using BankingSystem.PL.ViewModels.Admin;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.PL.Controllers.AppAdmin
{
    public class AdminController : Controller
    {
        private readonly IGenericRepository<Admin> _genericRepositoryAdmin;
        private readonly ISearchPaginationRepo<Admin> _searchPaginationAdmin;
        private readonly IUnitOfWork _unitOfWork;

        public AdminController(IGenericRepository<Admin> genericRepositoryAdmin, ISearchPaginationRepo<Admin> searchPaginationAdmin, IUnitOfWork unitOfWork)
        {
            _genericRepositoryAdmin = genericRepositoryAdmin;
            _searchPaginationAdmin = searchPaginationAdmin;
            _unitOfWork = unitOfWork;
        }


        // GET: AdminController
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Dashboard()
        {
            var transactions = _unitOfWork.Repository<Transaction>()
                .GetAllIncluding(t => t.Account, t => t.Payment, t => t.Customer)
                    .Where(t => t.Type == TransactionType.Withdraw || t.Type == TransactionType.Deposit || t.Type == TransactionType.LoanPayment)
                    .OrderByDescending(t => t.Payment.PaymentDate)
                    .Take(5)
                    .ToList();


            var Branches = _unitOfWork.Repository<Branch>().GetAll().ToList().Count;
            var ActiveAccounts = _unitOfWork.Repository<Account>().GetAll().Where(a => a.AccountStatus == AccountStatus.Active).ToList().Count;
            var holdings = _unitOfWork.Repository<Savings>().GetAll().ToList().Count;
            var TodayTransactions = _unitOfWork.Repository<Transaction>().GetAllIncluding(p => p.Payment)
                                                                            .Where(t => t.Payment.PaymentDate.Date.Day == DateTime.Now.Date.Day)
                                                                            .ToList().Count;


            ViewBag.TodayTransactions = TodayTransactions;
            ViewBag.Branches = Branches;
            ViewBag.ActiveAccounts = ActiveAccounts;
            ViewBag.Holdings = holdings;

            return View(transactions);


        }

        public IActionResult First_Four_Cards()
        {

            var Branches = _unitOfWork.Repository<Branch>().GetAll().ToList().Count;
            var ActiveAccounts = _unitOfWork.Repository<Account>().GetAll().Where(a => a.AccountStatus == AccountStatus.Active).ToList().Count;
            var holdings = _unitOfWork.Repository<Savings>().GetAll().ToList().Count;
            var TodayTransactions = _unitOfWork.Repository<Transaction>().GetAllIncluding(p => p.Payment)
                                                                            .Where(t => t.Payment.PaymentDate.Date.Day == DateTime.Now.Date.Day)
                                                                            .ToList().Count;

            var model = new MainDashboardFourCards()
            {
                TodayTransactions = TodayTransactions,
                Branches = Branches,
                ActiveAccounts = ActiveAccounts,
                Holdings = holdings
            };
            return View(model);
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
        public ActionResult Edit(string id)
        {
            return View();
        }

        // POST: AdminController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Admin admin)
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
