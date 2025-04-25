using AutoMapper;
using BankingSystem.DAL.Models;
using BankingSystem.PL.ViewModels.Auth;
using BankingSystem.PL.ViewModels.Customer;
using BankingSystem.PL.ViewModels.Manager;
using BankingSystem.PL.ViewModels.Teller;

namespace BankingSystem.PL.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterViewModel, ApplicationUser>()
                  .ForMember(d => d.Discriminator, o => o.MapFrom(s => s.Role))
                  .ForMember(d => d.PasswordHash, o => o.MapFrom(s => s.Password))
                  .ReverseMap();

            CreateMap<RegisterViewModel, Customer>()
                   .IncludeBase<RegisterViewModel, ApplicationUser>();

            CreateMap<RegisterViewModel, Admin>()
                .IncludeBase<RegisterViewModel, ApplicationUser>();

            CreateMap<RegisterViewModel, Manager>()
                .IncludeBase<RegisterViewModel, ApplicationUser>();

            CreateMap<RegisterViewModel, Teller>()
                .IncludeBase<RegisterViewModel, ApplicationUser>();

            CreateMap<SavingsViewModel, Savings>()
                   .ForPath(dest => dest.Branch.Name, opt => opt.MapFrom(src => src.BranchName ?? string.Empty))
                   .ReverseMap()
                   .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.Branch != null ? src.Branch.Name : string.Empty));


            CreateMap<Teller, TellerDetailsViewModel>()
           .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.Branch.Name))
           .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.Name));


            CreateMap<TellerDetailsViewModel, Teller>()
            .ForMember(dest => dest.Branch, opt => opt.Ignore())
            .ForMember(dest => dest.Department, opt => opt.Ignore());

            CreateMap<Customer, CustomersViewModel>()
                    .ForMember(dest => dest.Branch, s => s.MapFrom(s => s.Branch.Name));

            CreateMap<Customer, CustomerDetailsViewModel>()
                .ForMember(dest => dest.Branch, opt => opt.MapFrom(src => src.Branch.Name))
                .ForMember(dest => dest.Accounts, opt => opt.MapFrom(src => src.Accounts))
                .ForMember(dest => dest.Loans, opt => opt.MapFrom(src => src.Loans))
                .ForMember(dest => dest.Transactions, opt => opt.MapFrom(src => src.Transactions))
                .ForMember(dest => dest.Tickets, opt => opt.MapFrom(src => src.SupportTickets))
                .ForMember(dest => dest.TransactionsCount, opt => opt.MapFrom(src =>
                    src.Accounts != null ?
                    src.Accounts.Sum(a => a.AccountTransactions != null ? a.AccountTransactions.Count : 0) : 0))
                .ForMember(dest => dest.SupportTicketsCount, opt => opt.MapFrom(src =>
                    src.Accounts != null ?
                    src.Accounts.Sum(a => a.SupportTickets != null ? a.SupportTickets.Count : 0) : 0))
                .ForMember(dest => dest.LoansCount, opt => opt.MapFrom(src =>
                    src.Accounts != null ?
                    src.Accounts.Sum(a => a.Loans != null ? a.Loans.Count : 0) : 0))
                .ForMember(dest => dest.CertificatesCount, opt => opt.MapFrom(src =>
                    src.Accounts != null ?
                    src.Accounts.Sum(a => a.Certificates != null ? a.Certificates.Count : 0) : 0));


            CreateMap<Customer, ManagerCustomerDetailsViewModel>()
                .ForMember(dest => dest.LoanDetails, opt => opt.MapFrom(src => src.Loans != null && src.Loans.Any()
                    ? src.Loans.Select(l => new LoanDetail
                    {
                        LoanType = l.LoanType.ToString(),
                        LoanStatus = l.LoanStatus.ToString()
                    }).ToList()
                    : new List<LoanDetail> { new LoanDetail { LoanType = "No loans available", LoanStatus = "" } }))

                .ForMember(dest => dest.AccountDetails, opt => opt.MapFrom(src => src.Accounts != null && src.Accounts.Any()
                    ? src.Accounts.Select(a => new AccountDetail
                    {
                        AccountNumber = a.Number.ToString(),
                        AccountType = a.AccountType.ToString(),
                        AccountStatus = a.AccountStatus.ToString()
                    }).ToList()
                    : new List<AccountDetail> { new AccountDetail { AccountNumber = "No accounts available", AccountType = "", AccountStatus = "" } }))

                .ForMember(dest => dest.CardDetails, opt => opt.MapFrom(src => src.Accounts != null && src.Accounts.Select(a => a.Card).Any(c => c != null)
                    ? src.Accounts.Where(a => a.Card != null).Select(a => new CardDetail
                    {
                        CardType = a.Card.CardType.ToString(),
                        Number = a.Card.Number
                    }).ToList()
                    : new List<CardDetail> { new CardDetail { CardType = "No cards available", Number = "************" } }))

                .ForMember(dest => dest.SupportTicketDetails, opt => opt.MapFrom(src => src.SupportTickets != null && src.SupportTickets.Any()
                    ? src.SupportTickets.Select(st => new SupportTicketDetail
                    {
                        Status = st.Status.ToString(),
                        Type = st.Type.ToString()
                    }).ToList()
                    : new List<SupportTicketDetail> { new SupportTicketDetail { Status = "No support tickets available", Type = "" } }))

                .ForMember(dest => dest.TransactionDetails, opt => opt.MapFrom(src => src.Transactions != null && src.Transactions.Any()
                    ? src.Transactions.Select(t => new TransactionDetail
                    {
                        TransactionType = t.Type.ToString()
                    }).ToList()
                    : new List<TransactionDetail> { new TransactionDetail { TransactionType = "No transactions available" } }))

            .ForMember(dest => dest.CertificateDetails, opt => opt.MapFrom(src =>
                src.Accounts != null && src.Accounts.SelectMany(a => a.Certificates).Any()
                    ? src.Accounts.SelectMany(a => a.Certificates).Select(c => new CertificateDetail
                    {
                        CertificateNumber = c.CertificateNumber,
                        IssueDate = c.IssueDate,
                        ExpiryDate = c.ExpiryDate,
                        Amount = (double)c.Amount,
                        InterestRate = (double)c.GeneralCertificate.InterestRate

                    }).ToList()
                    : new List<CertificateDetail>
                    {
                        new CertificateDetail { CertificateNumber = "No Certificate available" }
                    }));


            CreateMap<Certificate, CertificateDetail>()
                .ForMember(dest => dest.AccountNumber, opt => opt.MapFrom(src => src.Account.Number))
                .ForMember(des => des.InterestRate, opt => opt.MapFrom(src => src.GeneralCertificate.InterestRate));

            CreateMap<Loan, LoanViewModel>()
                .ForMember(dest => dest.LoanStatus, opt => opt.MapFrom(src => src.LoanStatus))
                .ForMember(dest => dest.LoanType, opt => opt.MapFrom(src => src.LoanType))
                .ForMember(dest => dest.Payments, opt => opt.MapFrom(src => src.Payments))
                .ForMember(dest => dest.AccountNumber, opt => opt.MapFrom(src => src.Account.Number));


            CreateMap<Payment, PaymentViewModel>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));

            CreateMap<Account, CustomerAccountViewModel>()
                .ForMember(dest => dest.AccountType, opt => opt.MapFrom(src => src.AccountType != null ? src.AccountType.ToString() : "No data"))
                .ForMember(dest => dest.AccountStatus, opt => opt.MapFrom(src => src.AccountStatus != null ? src.AccountStatus.ToString() : "No data"));

            CreateMap<VisaCard, CustomerCardViewModel>()
                .ForMember(dest => dest.CardType, opt => opt.MapFrom(src => src.CardType != null ? src.CardType.ToString() : "No Card Data"))
                .ForMember(dest => dest.AccountNumber, opt => opt.MapFrom(src => src.Account.Number));


            CreateMap<SupportTicket, CustomerSupportTicketViewModel>()
                .ForMember(dest => dest.Response, opt => opt.MapFrom(src =>
                    !string.IsNullOrEmpty(src.Response) ? src.Response :
                    (src.Status == SupportTicketStatus.Pending ? "Still Working on it" :
                    (src.Status == SupportTicketStatus.Denied ? "Your ticket has been denied." : null))))
                .ForMember(dest => dest.AccountNumber, opt => opt.MapFrom(src =>
                    src.Account != null ? src.Account.Number : 0));



            CreateMap<Transaction, CustomerTransactionViewModel>()
                .ForMember(dest => dest.AccountNumber, opt => opt.MapFrom(src => src.Account.Number))
                .ForMember(dest => dest.AccountDestinatoin, opt => opt.MapFrom(src => src.AccountDistenationNumber))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
                .ForMember(dest => dest.DoneVia, opt => opt.MapFrom(src => src.DoneVia));


            CreateMap<Loan, LoansViewModel>()
                    .ForMember(dest => dest.CustomerName, s => s.MapFrom(s => s.Customer.UserName))
                    .ForMember(dest => dest.AccountNumber, s => s.MapFrom(s => s.Account.Number))
                    .ForMember(dest => dest.SSN, opt => opt.MapFrom(src => src.Customer.SSN))
                    .ReverseMap();

            CreateMap<Loan, LoanDetailsViewModel>()
                .ForMember(dest => dest.Loan, opt => opt.MapFrom(src => src)) // Maps Loan to LoansViewModel
                .ForMember(dest => dest.FinancialDocument, opt => opt.MapFrom(src => src.Customer.FinancialDocument)); // Assuming Customer has FinancialDocuments list

            CreateMap<LoanDetailsViewModel, Loan>()
                .ForMember(dest => dest.Customer, opt => opt.Ignore()) // Typically you wouldn't map this back
                .ForMember(dest => dest.Payments, opt => opt.MapFrom(src => src.Payments));


            CreateMap<SupportTicket, TicketsViewModel>()
                    .ForMember(dest => dest.CustomerName, s => s.MapFrom(s => s.Customer.UserName))
                    .ForMember(dest => dest.CustomerAccountNumber, s => s.MapFrom(s => s.Account.Number))
                    .ForMember(dest => dest.TellerName, s => s.MapFrom(s => s.Teller.UserName))
                    .ReverseMap();

            CreateMap<SupportTicket, TicketDetailsView>()
                    .ForMember(dest => dest.Ticket, opt => opt.MapFrom(src => src))
                    //.ForMember(dest => dest.Document, opt => opt.MapFrom(src => src.Customer.FinancialDocument)) // Or whatever logic you need
                    .ReverseMap();

            //CreateMap<SupportTicket, TicketDetailsView>()
            //        .ForMember(dest => dest.Ticket, opt => opt.MapFrom(src => src))
            //        .ForMember(dest => dest.Document, opt => opt.MapFrom(src => src.Customer.FinancialDocument))
            //        .ReverseMap();

            CreateMap<Account, AccountsViewModel>()
            .ForMember(dest => dest.SelectedAccountNumber, opt => opt.MapFrom(src => src.Number))
            .ForMember(dest => dest.SelectedCardNumber, opt => opt.MapFrom(src => src.Card.Number))
            .ForMember(dest => dest.VisaCVV, opt => opt.MapFrom(src => src.Card.CVV))
            .ForMember(dest => dest.VisaExpDate, opt => opt.MapFrom(src => src.Card.ExpDate))
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));

            CreateMap<VisaCard, CardsViewModel>()

            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.CardType, opt => opt.MapFrom(src => src.CardType.ToString()))
            .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src =>  src.Account.Customer.FirstName + " " + src.Account.Customer.LastName))
            .ForMember(dest => dest.AccountNumber, opt => opt.MapFrom(src => src.Account.Number));
            CreateMap<Account, AccountMinimal>()
                .ForMember(dest => dest.Number, opt => opt.MapFrom(src => src.Number))
                .ForMember(dest => dest.Balance, opt => opt.MapFrom(src => src.Balance ?? 0)) // Default to 0 if null
                .ForMember(dest => dest.AccountType, opt => opt.MapFrom(src => src.AccountType));

            CreateMap<Customer, CustomerViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.JoinDate, opt => opt.MapFrom(src => src.JoinDate))
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.BirthDate))
                .ReverseMap();

            CreateMap<Transaction, TransactionMinimal>()
                .ForMember(dest => dest.TransactionStatus, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Payment != null ? src.Payment.Amount : 0))
                .ForMember(dest => dest.PaymentDate, opt => opt.MapFrom(src => src.Payment != null ? src.Payment.PaymentDate : DateTime.MinValue));
            CreateMap<Customer, CustomerProfileViewModel>()
    .ForMember(dest => dest.DesiredCustomer, opt => opt.MapFrom(src => src)) // This will use the Customer→CustomerViewModel mapping
    .ForMember(dest => dest.Account, opt => opt.MapFrom(src => src.Accounts)) // This will use the Account→AccountMinimal mapping
    .ForMember(dest => dest.Transactions, opt => opt.MapFrom(src => src.Transactions)); // This will use Transaction→TransactionMinimal

            CreateMap<VisaCard, CustomerCardsViewModel>()
                    .ReverseMap()
                    .ForMember(dest => dest.AccountId, opt => opt.Ignore())
                    .ForMember(dest => dest.Account, opt => opt.Ignore());

            CreateMap<Loan, CustomerLoansViewModel>()
              .ReverseMap()
              .ForMember(dest => dest.Payments, opt => opt.Ignore())
              .ForMember(dest => dest.AccountId, opt => opt.Ignore())
              .ForMember(dest => dest.Account, opt => opt.Ignore())
              .ForMember(dest => dest.CustomerId, opt => opt.Ignore())
              .ForMember(dest => dest.Customer, opt => opt.Ignore())
              .ForMember(dest => dest.BranchId, opt => opt.Ignore())
              .ForMember(dest => dest.Branch, opt => opt.Ignore());

            CreateMap<SupportTicket, CustomerSupportTicket>()
            .ReverseMap()
            .ForMember(dest => dest.CustomerId, opt => opt.Ignore())  // Set separately
            .ForMember(dest => dest.TellerId, opt => opt.Ignore())    // Set separately
            .ForMember(dest => dest.Customer, opt => opt.Ignore())
            .ForMember(dest => dest.Teller, opt => opt.Ignore());

            CreateMap<Account, CustomerAccountsViewModel>()
               .ReverseMap()
               .ForMember(dest => dest.AccountTransactions, opt => opt.Ignore())
               .ForMember(dest => dest.Certificates, opt => opt.Ignore())
               .ForMember(dest => dest.Loans, opt => opt.Ignore())
               .ForMember(dest => dest.Customer, opt => opt.Ignore())
               .ForMember(dest => dest.Branch, opt => opt.Ignore());

            CreateMap<GeneralCertificate, GeneralCertificatesViewModel>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name ?? string.Empty))
                .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.Duration))
                .ForMember(dest => dest.InterestRate, opt => opt.MapFrom(src => src.InterestRate));

            CreateMap<Certificate, CustomerCertificatesViewModel>()
                .ForMember(dest => dest.GeneralCertDetails,
                    opt => opt.MapFrom(src => src.GeneralCertificate))
                .ForMember(dest => dest.Amount,
                    opt => opt.MapFrom(src => src.Amount ?? 0))
                .ForMember(dest => dest.IssueDate,
                    opt => opt.MapFrom(src => src.IssueDate))
                .ForMember(dest => dest.ExpiryDate,
                    opt => opt.MapFrom(src => src.ExpiryDate));

        }


    }
}
