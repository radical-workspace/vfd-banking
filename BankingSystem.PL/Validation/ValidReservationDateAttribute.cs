using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using BankingSystem.DAL.Data;
using BankingSystem.DAL.Models;
using BankingSystem.PL.ViewModels.Customer;
using Microsoft.EntityFrameworkCore;

namespace BankingSystem.PL.Validation
{
    public class ValidReservationDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var reservationVM = (ReservationViewModel)validationContext.ObjectInstance;
            var context = (BankingSystemContext)validationContext.GetService(typeof(BankingSystemContext));

            if (reservationVM.ReservationDate < DateTime.Now)
            {
                return new ValidationResult("Reservation date cannot be in the past.");
            }

            if (reservationVM.ReservationDate.DayOfWeek == DayOfWeek.Friday ||
                reservationVM.ReservationDate.DayOfWeek == DayOfWeek.Saturday)
            {
                return new ValidationResult("Reservation cannot be on Friday or Saturday.");
            }

            var branch = context.Branches.FirstOrDefault(b => b.Id == reservationVM.BranchId);

            if (branch == null || branch.Opens == null || branch.Closes == null)
            {
                return new ValidationResult("Invalid branch or branch working hours are not defined.");
            }

            var reservationTime = reservationVM.ReservationDate.TimeOfDay;

            if (reservationTime < branch.Opens || reservationTime >= branch.Closes)
            {
                var opens = branch.Opens.ToString(@"hh\:mm");
                var closes = branch.Closes.ToString(@"hh\:mm");

                return new ValidationResult($"Reservation time is outside the allowed working hours. Working hours are from {opens} to {closes}.");
            }

            var existingReservation = context.Reservations
                .AsNoTracking()
                .Where(r =>
                    r.BranchId == reservationVM.BranchId &&
                    r.ReservationDate.Date == reservationVM.ReservationDate.Date)
                .OrderBy(r => r.ReservationDate)
                .ToList();

            foreach (var res in existingReservation)
            {
                var differenceInMinutes = Math.Abs((reservationVM.ReservationDate - res.ReservationDate).TotalMinutes);

                if (differenceInMinutes < 30)
                {
                    var nextAvailableTime = res.ReservationDate.AddMinutes(30);
                    var remainingTime = nextAvailableTime - reservationVM.ReservationDate;

                    var remainingMinutes = (int)remainingTime.TotalMinutes;
                    return new ValidationResult($"You need to wait 30 minutes after the previous reservation. " +
                        $"You can book at {nextAvailableTime:HH:mm}. Time left: {remainingMinutes} minutes.");
                }
            }

            return ValidationResult.Success;
        }
    }
}
