using AutoMapper;
using SmartWealth.AuthService.Models;
using SmartWealth.AuthService.ViewModels;
using SmartWealth.AuthService.ViewModels.DTO;

namespace SmartWealth.AuthService.Utilities.Mappers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<UserResponse, User>().ReverseMap();
        CreateMap<UserLoginViewModel, User>().ReverseMap();
        CreateMap<UserRegistrationViewModel, User>().ReverseMap();
    }
}