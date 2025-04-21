using System.Security.Claims;
using BankingSystem.BLL.Interfaces;
using BankingSystem.DAL.Models;
using BankingSystem.PL.ViewModels.Customer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;


namespace BankingSystem.PL.Controllers.AppCustomer
{
    public class ReservationController(IUnitOfWork unitOfWork) : Controller
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        [HttpGet]
        public IActionResult CreateReservation()
        {
            var branches = _unitOfWork.Repository<Branch>().GetAll().ToList();
            ViewBag.Branches = branches;

            var locations = branches.Select(b => new
            {
                lat = b.Location.Split(',')[0],
                lng = b.Location.Split(',')[1],
                title = b.Name,
                branchId = b.Id,
                branchName = b.Name
            }).ToList();

            ViewBag.Locations = locations;
            return View();
        }
        [HttpPost]
        public IActionResult CreateReservation(ReservationViewModel reservationView)
        {
            if (!ModelState.IsValid)
            {
                var branches = _unitOfWork.Repository<Branch>().GetAll().ToList();
                ViewBag.Branches = branches;

                var locations = branches.Select(b => new
                {
                    lat = b.Location.Split(',')[0],
                    lng = b.Location.Split(',')[1],
                    title = b.Name,
                    branchId = b.Id,
                    branchName = b.Name
                });

                ViewBag.Locations = locations;

                return View(reservationView);
            }


            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                reservationView.ServiceType = ServiceType.OpenAccount;
            }
            else
            {
                reservationView.ServiceType = reservationView.ServiceType;
            }

            var reservation = new Reservation
            {
                CustomerId = userId,
                BranchId = reservationView.BranchId,
                ReservationDate = reservationView.ReservationDate,
                ServiceType = reservationView.ServiceType,
                Notes = reservationView.Notes,
                CreatedAt = DateTime.Now,
                Status = userId == null && reservationView.ServiceType == ServiceType.OpenAccount
                    ? ReservationStatus.Approved
                    : ReservationStatus.Pending
            };

            _unitOfWork.Repository<Reservation>().Add(reservation);
            _unitOfWork.Complete();

            if (userId == null && reservation.Status == ReservationStatus.Approved)
            {
                TempData["SuccessMessage"] = $"Your reservation has been successfully created " +
                                             $"You can view your QR Code <a href='{Url.Action("ReservationSuccess", "Reservation", new { reservationId = reservation.Id })}'>here</a>.";
            }
            else
            {
                TempData["SuccessMessage"] = "Your reservation has been successfully created. Please wait for confirmation.";
            }
            return RedirectToAction("CreateReservation");
        }
        [HttpGet]
        public IActionResult ReservationSuccess(int reservationId)
        {
            var reservation = _unitOfWork.Repository<Reservation>().Get(reservationId);
            if (reservation == null)
            {
                return NotFound();
            }

            var branch = _unitOfWork.Repository<Branch>().Get(reservation.BranchId);

            var branchName = branch != null ? branch.Name : "No Branch Assigned";

            var reservationDetails = $"Reservation ID: {reservation.Id}, Branch: {branchName}, Date: {reservation.ReservationDate}";

            using (var qrGenerator = new QRCodeGenerator())
            {
                var qrCodeData = qrGenerator.CreateQrCode(reservationDetails, QRCodeGenerator.ECCLevel.Q);
                var qrCode = new QRCode(qrCodeData);

                using (Bitmap qrBitmap = qrCode.GetGraphic(20))
                using (var ms = new MemoryStream())
                {
                    qrBitmap.Save(ms, ImageFormat.Png);
                    var qrImageBytes = ms.ToArray();
                    ViewBag.QrCodeImage = Convert.ToBase64String(qrImageBytes);
                }
            }
            //return User.Identity?.IsAuthenticated == true ? View(reservation) : RedirectToAction("Index", "Home");
            return View(reservation);

        }
        [HttpGet]
        public IActionResult DownloadQrCode(int reservationId)
        {
            var reservation = _unitOfWork.Repository<Reservation>().Get(reservationId);
            if (reservation == null)
            {
                return NotFound();
            }

            var branch = _unitOfWork.Repository<Branch>().Get(reservation.BranchId);
            var branchName = branch != null ? branch.Name : "No Branch Assigned";

            var reservationDetails = $"Reservation ID: {reservation.Id}, Branch: {branchName}, Date: {reservation.ReservationDate}";

            using (var qrGenerator = new QRCodeGenerator())
            {
                var qrCodeData = qrGenerator.CreateQrCode(reservationDetails, QRCodeGenerator.ECCLevel.Q);
                var qrCode = new QRCode(qrCodeData);

                using (Bitmap qrBitmap = qrCode.GetGraphic(20))
                using (var ms = new MemoryStream())
                {
                    qrBitmap.Save(ms, ImageFormat.Png);
                    byte[] qrImageBytes = ms.ToArray();

                    return File(qrImageBytes, "image/png", "ReservationQRCode.png");
                }
            }
        }
        [HttpGet]
        public IActionResult GetCustomerAllReservations()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var reservations = _unitOfWork.Repository<Reservation>().GetAllIncluding(r => r.Branch).Where(r => r.CustomerId == userId)
                .OrderByDescending(r => r.ReservationDate).ToList();
            return View(reservations);
        }

    }
}
