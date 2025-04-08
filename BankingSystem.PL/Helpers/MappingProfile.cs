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

            CreateMap<RegisterViewModel, Customer>()
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

            CreateMap<Customer, CustomersViewModel>()
                    .ForMember(dest => dest.Id, s => s.MapFrom(s => s.Id))
                    .ForMember(dest => dest.FirstName, s => s.MapFrom(s => s.FirstName))
                    .ForMember(dest => dest.LastName, s => s.MapFrom(s => s.LastName))
                    .ForMember(dest => dest.Address, s => s.MapFrom(s => s.Address))
                    .ForMember(dest => dest.JoinDate, s => s.MapFrom(s => s.JoinDate))
                    .ForMember(dest => dest.BirthDate, s => s.MapFrom(s => s.BirthDate))
                    .ForMember(dest => dest.Branch, s => s.MapFrom(s => s.Branch.Name));

            CreateMap<Customer, CustomerDetailsViewModel>()
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




        }


    }
}
