﻿using AutoMapper;
using SmartWealth.AuthService.Models;
using SmartWealth.AuthService.ViewModels;

namespace SmartWealth.AuthService.Utilities.Mappers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<UserResponse, User>().ReverseMap();
        CreateMap<UserViewModel, User>().ReverseMap();
    }
}