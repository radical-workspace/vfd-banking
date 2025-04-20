using AutoMapper;
using BankingSystem.BLL.Interfaces;
using BankingSystem.DAL.Models;
using BankingSystem.PL.ViewModels.Admin;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.PL.Controllers.AppAdmin
{
    public class AdminMainDashboardController(IUnitOfWork unitOfWork) : Controller
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        //public IActionResult First_Four_Cards()
        //{

        //    var Branches = _unitOfWork.Repository<Branch>().GetAll().ToList().Count;
        //    var ActiveAccounts = _unitOfWork.Repository<Account>().GetAll().Where(a => a.AccountStatus == AccountStatus.Active).ToList().Count;
        //    var holdings = _unitOfWork.Repository<Savings>().GetAll().ToList().Count;
        //    var TodayTransactions = _unitOfWork.Repository<Transaction>().GetAllIncluding(p => p.Payment)
        //                                                                    .Where(t => t.Payment.PaymentDate.Date.Day == DateTime.Now.Date.Day)
        //                                                                    .ToList().Count;

        //    var model = new MainDashboardFourCards()
        //    {
        //        TodayTransactions = TodayTransactions,
        //        Branches = Branches,
        //        ActiveAccounts = ActiveAccounts,
        //        Holdings = holdings
        //    };
        //    return View(model);
        //}
    }
}
