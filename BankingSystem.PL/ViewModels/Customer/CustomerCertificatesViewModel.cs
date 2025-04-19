using System.ComponentModel.DataAnnotations;

namespace BankingSystem.PL.ViewModels.Customer
{
    public class CustomerCertificatesViewModel 
    {
        public GeneralCertificatesViewModel GeneralCertDetails { get; set; } = new ();
        public double Amount { get; set; } = 0;
        public DateTime IssueDate { get; set; }
        public DateTime ExpiryDate { get; set; }
    }

    public class GeneralCertificatesViewModel
    {
        public string Name { get; set; } = string.Empty;
        public int Duration { get; set; } = 0;
        public double InterestRate { get; set; } = 0;
    }
}
