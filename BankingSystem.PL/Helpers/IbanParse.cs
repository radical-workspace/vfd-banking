namespace BankingSystem.PL.Helpers
{
    public class IbanParser
    {
        public static long ExtractAccountNumber(string iban)
        {
            // Validate the IBAN to ensure it meets minimum length requirements
            if (string.IsNullOrWhiteSpace(iban) || iban.Length < 27)
                throw new ArgumentException("Invalid IBAN format. The IBAN must be at least 27 characters long.");

            // Remove spaces for consistent processing
            iban = iban.Replace(" ", "");

            // Define the starting index and length for the account number (specific to the IBAN format for Egypt)
            int accountStartIndex = 16; // Start after the first 16 characters (country code + check digits + bank code + branch code)
            int accountLength = 12;     // Account number is 11 digits long

            // Ensure the IBAN has enough characters to extract the account number
            if (iban.Length < accountStartIndex + accountLength)
                throw new ArgumentException("Invalid IBAN format. Insufficient characters to extract the account number.");

            // Extract the account number as a string
            string accountNumberString = iban.Substring(accountStartIndex, accountLength);

            // Parse the string into a long number
            if (!long.TryParse(accountNumberString, out long accountNumber))
                throw new FormatException("Account number contains invalid characters or exceeds the maximum value for a long.");

            return accountNumber;
        }
    }

}
