using BankingSystem.DAL.Data;
using System.ComponentModel.DataAnnotations;

namespace BankingSystem.PL.Validation
{
    public class UniqueEmail :ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var _dbContext = validationContext.GetService<BankingSystemContext>();
            if (_dbContext is null) return ValidationResult.Success;

            // Use dynamic to support different view models that contain Email and Id
            dynamic entity = validationContext.ObjectInstance;

            string currentEmail = value as string ?? string.Empty;
            string currentId = entity.Id;

            bool emailExists = _dbContext.Users.Any(u => u.Email == currentEmail && u.Id != currentId);
            if (emailExists)
                return new ValidationResult("This Email already exists");

            return ValidationResult.Success;
        }
    }
}
