using AutoMapper;
using BankingSystem.BLL.Interfaces;
using BankingSystem.DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.PL.Controllers
{
    public class CustomerTransfer(IUnitOfWork unitOfWork, IMapper mapper) : Controller
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public IActionResult TransferMoney()
        {
            var Accounts = _unitOfWork.Repository<Account>().GetAllIncluding(c => c.Customer!, a => a.Card);
            if (Accounts == null) return NotFound();

            List<AccountsViewModel> AccountsVM = _mapper.Map<List<AccountsViewModel>>(Accounts);

            return View();
        }

        [HttpPost]
        public IActionResult TransferMoney(TransferMoneyViewModel transferMoneyVM)
        {
            if (!ModelState.IsValid)
            {
                return View(transferMoneyVM);
            }
            var senderAccount = _unitOfWork.Repository<Account>().GetById(transferMoneyVM.SenderAccountId);
            var receiverAccount = _unitOfWork.Repository<Account>().GetById(transferMoneyVM.ReceiverAccountId);
            if (senderAccount == null || receiverAccount == null)
            {
                ModelState.AddModelError("", "Invalid account details.");
                return View(transferMoneyVM);
            }
            if (senderAccount.Balance < transferMoneyVM.Amount)
            {
                ModelState.AddModelError("", "Insufficient balance.");
                return View(transferMoneyVM);
            }
            senderAccount.Balance -= transferMoneyVM.Amount;
            receiverAccount.Balance += transferMoneyVM.Amount;
            _unitOfWork.Repository<Account>().Update(senderAccount);
            _unitOfWork.Repository<Account>().Update(receiverAccount);
            _unitOfWork.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
    }
}
