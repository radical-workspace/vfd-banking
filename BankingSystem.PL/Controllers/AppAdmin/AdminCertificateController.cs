using AutoMapper;
using BankingSystem.BLL.Interfaces;
using BankingSystem.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BankingSystem.PL.Controllers.AppAdmin
{

    public class AdminCertificateController(IUnitOfWork unitOfWork, IMapper mapper) : Controller
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;


        // GET: GeneralCertificates
        public IActionResult Index() => View(_unitOfWork.Repository<GeneralCertificate>().GetAll());


        // GET: GeneralCertificates/Details/5
        public IActionResult Details(int id)
        {
            var generalCertificate = _unitOfWork.Repository<GeneralCertificate>().Get(id);
            return View(generalCertificate);
        }

        // GET: GeneralCertificates/Create
        public IActionResult Create() => View();

        // POST: GeneralCertificates/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Name,Duration,InterestRate,Amount")] GeneralCertificate generalCertificate)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Repository<GeneralCertificate>().Add(generalCertificate);
                _unitOfWork.Complete();
                return RedirectToAction(nameof(Index));
            }
            return View(generalCertificate);
        }

        // GET: GeneralCertificates/Edit/5
        public IActionResult Edit(int id)
        {
            var generalCertificate = _unitOfWork.Repository<GeneralCertificate>().Get(id);
            if (generalCertificate == null)
                return NotFound();

            return View(generalCertificate);
        }

        // POST: GeneralCertificates/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Name,Duration,InterestRate,Amount")] GeneralCertificate generalCertificate)
        {
            if (id != generalCertificate.Id) return NotFound();


            if (ModelState.IsValid)
            {
                _unitOfWork.Repository<GeneralCertificate>().Update(generalCertificate);
                _unitOfWork.Complete();
                return RedirectToAction(nameof(Index));
            }
            return View(generalCertificate);
        }

        // GET: GeneralCertificates/Delete/5
        public IActionResult Delete(int id)
        {
            var generalCertificate = _unitOfWork.Repository<GeneralCertificate>().Get(id);

            if (generalCertificate == null) return NotFound();

            return View(generalCertificate);
        }

        // POST: GeneralCertificates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var generalCertificate = _unitOfWork.Repository<GeneralCertificate>().Get(id);

            if (generalCertificate != null) _unitOfWork.Repository<GeneralCertificate>().Delete(generalCertificate);

            _unitOfWork.Complete();
            return RedirectToAction(nameof(Index));
        }
    }
}