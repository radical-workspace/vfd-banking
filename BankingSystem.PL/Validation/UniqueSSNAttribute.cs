using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using BankingSystem.DAL.Data;

namespace BankingSystem.PL.Validation
{
    public class UniqueSSNAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var ssn = value as long?;
            if (ssn == null)
                return new ValidationResult("SSN is required");

            var dbContext = validationContext.GetService<BankingSystemContext>();

            var currentObject = validationContext.ObjectInstance;

            string? currentUserId = null;

            if (currentObject.GetType().GetProperty("Id") != null)
            {
                currentUserId = currentObject.GetType().GetProperty("Id")?.GetValue(currentObject)?.ToString();
            }

            var user = dbContext.Users.FirstOrDefault(u =>
                u.SSN == ssn &&
                u.Id != currentUserId);

            if (user != null)
            {
                return new ValidationResult("This SSN already exists.");
            }

            return ValidationResult.Success;
        }
    }
}
