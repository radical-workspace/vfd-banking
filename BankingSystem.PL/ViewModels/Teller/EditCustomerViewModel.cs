using BankingSystem.PL.Validation;

namespace BankingSystem.PL.ViewModels.Teller
{
    public class EditCustomerViewModel
    {
        public string Id { get; set; }
        [UniqueEmail]
        public string Email { get; set; } = null!;
        [UniqueSSN]
        public long SSN { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Address { get; set; } = null!;
        public DateTime JoinDate { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
