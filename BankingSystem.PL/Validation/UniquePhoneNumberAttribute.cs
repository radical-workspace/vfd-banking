using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using BankingSystem.DAL.Data;

namespace BankingSystem.PL.Validation
{
    public class UniquePhoneNumberAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var phoneNumber = value as string;
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return new ValidationResult("Phone number is required.");

            var dbContext = validationContext.GetService<BankingSystemContext>();

            var currentObject = validationContext.ObjectInstance;
            string? currentUserId = null;

            if (currentObject.GetType().GetProperty("Id") != null)
            {
                currentUserId = currentObject.GetType().GetProperty("Id")?.GetValue(currentObject)?.ToString();
            }

            var user = dbContext.Users.FirstOrDefault(u =>
                                                      u.PhoneNumber == phoneNumber && u.Id != currentUserId);

            if (user != null)
            {
                return new ValidationResult("This phone number is already in use.");
            }

            return ValidationResult.Success;
        }
    }
}
