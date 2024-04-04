using AutoMapper;
using SmartWealth.AuthService.Models;
using SmartWealth.AuthService.ViewModels;

namespace SmartWealth.AuthService.Utilities.Mappers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<UserLoginViewModel, User>().ReverseMap();
        CreateMap<UserRegistrationViewModel, User>().ReverseMap();
    }
}