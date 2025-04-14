using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSystem.BLL.Services
{
    public class CertificateGenerator
    {
        public static string Create(long userId)
        {
            // Get the first 3 characters of the userId as a string
            string userIdPrefix = userId.ToString().Substring(0, Math.Min(3, userId.ToString().Length));

            string timestamp = DateTime.Now.ToString("yyyyMMdd-HHmmss");
            string randomDigits = new Random().Next(100, 999).ToString();

            return $"CERT-{timestamp}-{userIdPrefix}-{randomDigits}";
        }
    }

}
