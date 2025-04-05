using AutoMapper;
using BankingSystem.DAL.Models;
using BankingSystem.PL.ViewModels;

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

     


        }
    }
}
