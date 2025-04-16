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

            CreateMap<RegisterViewModel, MyCustomer>()
                                            .IncludeBase<RegisterViewModel, ApplicationUser>();

            CreateMap<RegisterViewModel, Admin>()
                .IncludeBase<RegisterViewModel, ApplicationUser>();

            CreateMap<RegisterViewModel, MyManager>()
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

            CreateMap<MyCustomer, CustomersViewModel>()
                    .ForMember(dest => dest.Id, s => s.MapFrom(s => s.Id))
                    .ForMember(dest => dest.FirstName, s => s.MapFrom(s => s.FirstName))
                    .ForMember(dest => dest.LastName, s => s.MapFrom(s => s.LastName))
                    .ForMember(dest => dest.Address, s => s.MapFrom(s => s.Address))
                    .ForMember(dest => dest.JoinDate, s => s.MapFrom(s => s.JoinDate))
                    .ForMember(dest => dest.BirthDate, s => s.MapFrom(s => s.BirthDate))
                    .ForMember(dest => dest.Branch, s => s.MapFrom(s => s.Branch.Name));

            CreateMap<MyCustomer, CustomerDetailsViewModel>()
                    .ForMember(dest => dest.Id, s => s.MapFrom(s => s.Id))
                    .ForMember(dest => dest.FirstName, s => s.MapFrom(s => s.FirstName))
                    .ForMember(dest => dest.LastName, s => s.MapFrom(s => s.LastName))
                    .ForMember(dest => dest.Address, s => s.MapFrom(s => s.Address))
                    .ForMember(dest => dest.JoinDate, s => s.MapFrom(s => s.JoinDate))
                    .ForMember(dest => dest.BirthDate, s => s.MapFrom(s => s.BirthDate))
                    .ForMember(dest => dest.Branch, s => s.MapFrom(s => s.Branch.Name))
                    .ForMember(dest => dest.Branch, opt => opt.MapFrom(src => src.Branch.Name))
                    .ForMember(dest => dest.AccountNumbers, opt => opt.MapFrom(src => src.Accounts.Select(a => a.Number)))
                    .ForMember(dest => dest.LoanTypes, opt => opt.MapFrom(src => src.Loans.Select(l => l.LoanType)))
                    .ForMember(dest => dest.CardTypes, opt => opt.MapFrom(src => src.Cards.Select(c => c.CardType)))
                    .ForMember(dest => dest.TransactionDescriptions, opt => opt.MapFrom(src => src.Transactions.Select(t => t.DoneVia)))
                    .ForMember(dest => dest.TicketSubjects, opt => opt.MapFrom(src => src.SupportTickets.Select(s => s.Description)))
                    .ReverseMap();



            
            CreateMap<Account, AccountMinimal>()
                .ForMember(dest => dest.Number, opt => opt.MapFrom(src => src.Number))
                .ForMember(dest => dest.Balance, opt => opt.MapFrom(src => src.Balance ?? 0)) // Default to 0 if null
                .ForMember(dest => dest.AccountType, opt => opt.MapFrom(src => src.AccountType));

            CreateMap<MyCustomer, CustomerViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.JoinDate, opt => opt.MapFrom(src => src.JoinDate))
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.BirthDate))
                .ReverseMap(); 

            CreateMap<MyCustomer, CustomerProfileViewModel>()
                .ForMember(dest => dest.DesiredCustomer, opt => opt.MapFrom(src => src))
                .ForMember(dest => dest.Account, opt => opt.MapFrom(src => src.Accounts));


            CreateMap<Card, CustomerCardsViewModel>()
                    .ReverseMap()
                    .ForMember(dest => dest.AccountId, opt => opt.Ignore())
                    .ForMember(dest => dest.Account, opt => opt.Ignore())
                    .ForMember(dest => dest.CustomerId, opt => opt.Ignore())
                    .ForMember(dest => dest.Customer, opt => opt.Ignore());

            CreateMap<Loan, CustomerLoansViewModel>()
              .ReverseMap()
              .ForMember(dest => dest.Payments, opt => opt.Ignore())
              .ForMember(dest => dest.AccountId, opt => opt.Ignore())
              .ForMember(dest => dest.Account, opt => opt.Ignore())
              .ForMember(dest => dest.CustomerId, opt => opt.Ignore())
              .ForMember(dest => dest.Customer, opt => opt.Ignore())
              .ForMember(dest => dest.BranchId, opt => opt.Ignore())
              .ForMember(dest => dest.Branch, opt => opt.Ignore());

            CreateMap<SupportTicket , CustomerSupportTicket>()
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
               .ForMember(dest => dest.Cards, opt => opt.Ignore())
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
