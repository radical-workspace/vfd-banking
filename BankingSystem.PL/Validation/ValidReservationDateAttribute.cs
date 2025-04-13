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

            var existingReservation = context.Reservations
                .AsNoTracking()
                .FirstOrDefault(r =>
                    r.BranchId == reservationVM.BranchId &&
                    r.ReservationDate == reservationVM.ReservationDate);

            if (existingReservation != null)
            {
                return new ValidationResult("There's already a reservation for this branch at this exact time.");
            }

            var branch = context.Branches.FirstOrDefault(b => b.Id == reservationVM.BranchId);

            if (branch == null || branch.Opens == null || branch.Closes == null)
            {
                return new ValidationResult("Invalid branch or branch working hours are not defined.");
            }

            var reservationTime = reservationVM.ReservationDate.TimeOfDay;

            if (reservationTime < branch.Opens || reservationTime > branch.Closes)
            {
                return new ValidationResult("Reservation time is outside the allowed working hours.");
            }

            return ValidationResult.Success;
        }
    }
}
