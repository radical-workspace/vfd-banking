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
                .ForMember(dest => dest.Tickets, opt => opt.MapFrom(src => src.SupportTickets));


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

                //.ForMember(dest => dest.CardDetails, opt => opt.MapFrom(src => src.Card != null && src.Card.Any()
                //    ? src.Card.Select(c => new CardDetail
                //    {
                //        CardType = c.CardType.ToString(),
                //        Number = c.Number.ToString()
                //    }).ToList()
                //    : new List<CardDetail> { new CardDetail { CardType = "No cards available", Number = "" } }))

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
                    : new List<TransactionDetail> { new TransactionDetail { TransactionType = "No transactions available" } })) ;

                    //.ForMember(dest => dest.CertificateDetails, opt => opt.MapFrom(src =>
                    //    src.Accounts != null && src.Accounts.SelectMany(a => a.Certificates).Any()
                    //        ? src.Accounts.SelectMany(a => a.Certificates).Select(c => new CertificateDetail
                    //        {
                    //            CertificateNumber = c.CertificateNumber,
                    //            IssueDate = c.IssueDate,
                    //            ExpiryDate = c.ExpiryDate,
                    //            Amount = c.Amount,
                    //            InterestRate = c.InterestRate
                    //        }).ToList()
                    //        : new List<CertificateDetail>
                    //        {
                    //            new CertificateDetail { CertificateNumber = "No Certificate available" }
                    //        }));


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
                .ForMember(dest => dest.CardType, opt => opt.MapFrom(src => src.CardType != null ? src.CardType.ToString(): "No Card Data"))
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

            CreateMap<Certificate, CertificateDetail>()
                .ForMember(dest => dest.AccountNumber, opt => opt.MapFrom(src => src.Account.Number));
            CreateMap<Loan, LoansViewModel>()
                    .ForMember(dest => dest.CustomerName, s => s.MapFrom(s => s.Customer.UserName))
                    .ForMember(dest => dest.AccountNumber, s => s.MapFrom(s => s.Account.Number))
                    .ForMember(dest=>dest.SSN,opt=>opt.MapFrom(src=>src.Customer.SSN))
                    .ReverseMap();

            CreateMap<Loan, LoanDetailsViewModel>()
                    .ForMember(dest => dest.Loan, opt => opt.MapFrom(src => src))
                    .ForMember(dest => dest.FinancialDocument, opt => opt.MapFrom(src => src.Customer.FinancialDocument));

            CreateMap<SupportTicket, TicketsViewModel>()
                    .ForMember(dest => dest.CustomerName, s => s.MapFrom(s => s.Customer.UserName))
                    .ForMember(dest => dest.CustomerAccountNumber, s => s.MapFrom(s => s.Account.Number))
                    .ForMember(dest => dest.TellerName, s => s.MapFrom(s => s.Teller.UserName))
                    .ReverseMap();

            CreateMap<SupportTicket, TicketDetailsView>()
                    .ForMember(dest => dest.Ticket, opt => opt.MapFrom(src => src))
                    .ForMember(dest => dest.Document, opt => opt.MapFrom(src => src.Customer.FinancialDocument))
                    .ReverseMap();

            CreateMap<Account, AccountsViewModel>()
            .ForMember(dest => dest.AccountNumber, opt => opt.MapFrom(src => src.Number))
            .ForMember(dest => dest.VisaNumber, opt => opt.MapFrom(src => src.Card.Number))
            .ForMember(dest => dest.VisaCVV, opt => opt.MapFrom(src => src.Card.CVV))
            .ForMember(dest => dest.VisaExpDate, opt => opt.MapFrom(src => src.Card.ExpDate))
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));

            CreateMap<VisaCard, CardsViewModel>()

            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.CardType, opt => opt.MapFrom(src => src.CardType.ToString()))
            //.ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Account.Customer.UserName))
            .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => $"{src.Account.Customer.FirstName} {src.Account.Customer.LastName}"))
            .ForMember(dest => dest.AccountNumber, opt => opt.MapFrom(src => src.Account.Number));
        }


    }
}
