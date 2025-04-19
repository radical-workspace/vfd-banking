using System.ComponentModel.DataAnnotations;

namespace BankingSystem.PL.Validation
{

     //Custom validation attribute for future dates
    //public class FutureDateAttribute : ValidationAttribute
    //{
    //    public override bool IsValid(object? value)
    //    {
    //        if (value is DateTime date)
    //        {
    //            return date > DateTime.Now;
    //        }
    //        return false;
    //    }
    //}

    // Custom validation attribute for conditional requirements
    public class RequiredWhenAttribute : ValidationAttribute
    {
        private readonly string _propertyName;
        private readonly object _targetValue;

        public RequiredWhenAttribute(string propertyName, object targetValue, string errorMessage = "")
        {
            _propertyName = propertyName;
            _targetValue = targetValue;
            ErrorMessage = errorMessage;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext context)
        {
            var instance = context.ObjectInstance;
            var property = instance.GetType().GetProperty(_propertyName);

            if (property != null)
            {
                var propertyValue = property.GetValue(instance);

                if (propertyValue?.Equals(_targetValue) == true && (value == null || string.IsNullOrWhiteSpace(value.ToString())))
                {
                    return new ValidationResult(ErrorMessage);
                }
            }

            return ValidationResult.Success;
        }
    }
}
