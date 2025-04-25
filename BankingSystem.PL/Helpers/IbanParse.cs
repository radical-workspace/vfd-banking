namespace BankingSystem.PL.Helpers
{
    public class IbanParser
    {
        public static long ExtractAccountNumber(string iban)
        {
            if (string.IsNullOrWhiteSpace(iban) || iban.Length < 27)
                throw new ArgumentException("Invalid IBAN format. The IBAN must be at least 27 characters long.");

            iban = iban.Replace(" ", "");

            int accountStartIndex = 16; // Start after the first 16 characters (country code + check digits + bank code + branch code)
            int accountLength = 12;     // Account number is 12 digits long

            // Ensure the IBAN has enough characters to extract the account number
            if (iban.Length < accountStartIndex + accountLength)
                throw new ArgumentException("Invalid IBAN format. Insufficient characters to extract the account number.");

            // Extract the account number as a string
            string accountNumberString = iban.Substring(accountStartIndex, accountLength);

            if (!long.TryParse(accountNumberString, out long accountNumber))
                throw new FormatException("Account number contains invalid characters or exceeds the maximum value for a long.");

            return accountNumber;
        }
    }

}
