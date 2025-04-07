using AutoMapper;
using BankingSystem.DAL.Models;
using BankingSystem.PL.ViewModels.Auth;
using BankingSystem.PL.ViewModels.Manager;

namespace BankingSystem.PL.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterViewModel, ApplicationUser>()
                  .ForMember(d => d.SSN, o => o.MapFrom(s => s.SSN))
                  .ForMember(d => d.FirstName, o => o.MapFrom(s => s.FirstName))
                  .ForMember(d => d.LastName, o => o.MapFrom(s => s.LastName))
                  .ForMember(d => d.PhoneNumber, o => o.MapFrom(s => s.PhoneNumber))
                  .ForMember(d => d.UserName, o => o.MapFrom(s => s.UserName))
                  .ForMember(d => d.Address, o => o.MapFrom(s => s.Address))
                  .ForMember(d => d.BirthDate, o => o.MapFrom(s => s.BirthDate))
                  .ForMember(d => d.JoinDate, o => o.MapFrom(s => s.JoinDate))
                  .ForMember(d => d.Email, o => o.MapFrom(s => s.Email))
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

            //CreateMap<SavingsViewModel, Savings>()
            //    .ForMember(dest => dest.Branch.Name, opt => opt.MapFrom(src => src.BranchName != null ? src.BranchName : string.Empty))
            //    .ReverseMap();
            CreateMap<SavingsViewModel, Savings>()
                    .ForPath(dest => dest.Branch.Name, opt => opt.MapFrom(src => src.BranchName ?? string.Empty))
                    .ReverseMap()
                    .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.Branch != null ? src.Branch.Name : string.Empty));

        }
    }
}
