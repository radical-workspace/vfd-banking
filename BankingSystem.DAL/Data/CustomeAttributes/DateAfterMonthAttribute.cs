using System;
using System.ComponentModel.DataAnnotations;


namespace BankingSystem.DAL.Data.CustomeAttributes
{
    public class DateAfterMonthAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        if (value is DateTime dateValue)
        {
            return dateValue >= DateTime.Now.AddMonths(1).Date;
        }
        return false;
    }

    public override string FormatErrorMessage(string name)
    {
        return $"{name} must be at least one month from today.";
    }
}
}
