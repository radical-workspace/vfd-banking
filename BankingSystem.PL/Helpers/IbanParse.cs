namespace BankingSystem.PL.Helpers
{
    public class IbanParser
    {
        public static string ExtractAccountNumber(string iban)
        {
            // Check if IBAN is null or too short
            if (string.IsNullOrWhiteSpace(iban) || iban.Length < 27)
            {
                throw new ArgumentException("Invalid IBAN format.");
            }

            // For Egypt, the account number starts after the first 8 characters (EG, check digits, and bank/branch codes)
            int accountStartIndex = 8;

            // Extract the account number from the IBAN
            string accountNumber = iban.Substring(accountStartIndex);

            return accountNumber;
        }
    }

}
