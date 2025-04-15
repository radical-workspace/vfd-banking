
using System.ComponentModel.DataAnnotations;


namespace BankingSystem.DAL.Data.CustomeAttributes
{
    public class DateAfterWeekAttribute : ValidationAttribute

    {
       
            public override bool IsValid(object value)
            {
                if (value is DateTime dateValue)
                {
                    return dateValue >= DateTime.Now.AddDays(7).Date;
                }
                return false;
            }

            public override string FormatErrorMessage(string name)
            {
                return $"{name} must be at least one week from today.";
            }
        }
    }

