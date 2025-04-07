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

            CreateMap<Teller, TellerDetailsViewModel>()
            .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.Branch.Name))
            .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.Name));


            CreateMap<TellerDetailsViewModel, Teller>()
            .ForMember(dest => dest.Branch, opt => opt.Ignore())   
            .ForMember(dest => dest.Department, opt => opt.Ignore())
            .ForMember(dest => dest.Id, opt => opt.Ignore());      

        }


    }
}
