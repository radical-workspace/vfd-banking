using AutoMapper;
using BankingSystem.BLL.Interfaces;
using BankingSystem.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BankingSystem.PL.Controllers.AppTeller
{
    public class HandleReservationController (IUnitOfWork unitOfWork,IMapper mapper): Controller
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public readonly IMapper _mapper = mapper;

        [HttpGet]
        public IActionResult GetBranchReservations()
        {
            var tellerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var teller = _unitOfWork.Repository<Teller>()
                .GetSingleIncluding(t => t.Id == tellerId);

            var branchId = teller.BranchId;

            var reservations = _unitOfWork.Repository<Reservation>()
                .GetAllIncluding(r => r.Customer)
                .Where(r => r.BranchId == branchId)
                .OrderByDescending(r => r.ReservationDate)
                .ToList();

            return View(nameof(GetBranchReservations),reservations);
        }

        [Authorize(Roles = "Teller")]
        [HttpPost]
        public IActionResult UpdateReservationStatus(int id, ReservationStatus status)
        {
            var reservation = _unitOfWork.Repository<Reservation>().Get(id);
            if (reservation == null)
                return NotFound();

            reservation.Status = status;
            _unitOfWork.Complete();

            return RedirectToAction("GetBranchReservations");
        }

    }
}
