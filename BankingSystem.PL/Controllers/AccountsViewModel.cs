using BankingSystem.DAL.Models;

namespace BankingSystem.PL.Controllers
{
    public class AccountsViewModel
    {
        public int Id { get; set; }
        public double? Balance { get; set; }
        public long AccountNumber { get; set; }

        /*
          Structure of an IBAN:
            An IBAN consists of:
            Country Code: A two-letter code representing the country (e.g., EG for Egypt).
            Check Digits: Two numbers that help validate the accuracy of the IBAN.
            Bank Code: Identifies the specific bank.
            Branch Code (optional in some countries): Specifies the branch of the bank.
            Account Number: The unique identifier for your account.
            How It’s Generated:
            Banks generate IBANs based on the following:
            They take the existing domestic account details.
            The country's standard IBAN format is applied.
            The bank adds the country code, check digits, and other required details according to the rules of the international banking system.
            Each country has its own IBAN length and format. For example, the IBAN format in Egypt is 27 characters long, starting with “EG.”
            example
            Here's an example of an IBAN for Egypt:
            EG38 1234 5678 9012 3456 7890 1234
            Here’s how the parts break down:
            EG: Country code for Egypt.
            38: Check digits used to validate the IBAN.
            1234: Bank code identifying the specific bank.
            5678 9012: Branch or region code.
            3456 7890 1234: Your unique account number.
         */

        public string DestinationIban { get; set; } = null!;

        public string VisaNumber { get; set; } = null!;

        public string VisaCVV { get; set; } = null!;

        public DateTime VisaExpDate { get; set; }

    }
}