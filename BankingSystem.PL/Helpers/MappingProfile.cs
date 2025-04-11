using AutoMapper;
using BankingSystem.DAL.Models;
using BankingSystem.PL.ViewModels.Auth;
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
                    .ForMember(dest => dest.Branch, s => s.MapFrom(s => s.Branch.Name));

            CreateMap<MyCustomer, CustomerDetailsViewModel>()
                    .ForMember(dest => dest.Branch, s => s.MapFrom(s => s.Branch.Name))
                    .ForMember(dest => dest.Branch, opt => opt.MapFrom(src => src.Branch.Name))
                    .ForMember(dest => dest.AccountNumbers, opt => opt.MapFrom(src => src.Accounts.Select(a => a.Number)))
                    .ForMember(dest => dest.LoanTypes, opt => opt.MapFrom(src => src.Loans.Select(l => l.LoanType)))
                    .ForMember(dest => dest.CardTypes, opt => opt.MapFrom(src => src.Cards.Select(c => c.CardType)))
                    .ForMember(dest => dest.TransactionDescriptions, opt => opt.MapFrom(src => src.Transactions.Select(t => t.DoneVia)))
                    .ForMember(dest => dest.TicketSubjects, opt => opt.MapFrom(src => src.SupportTickets.Select(s => s.Description)))
                    .ReverseMap();

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
        }


    }
}
