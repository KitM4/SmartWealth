using AutoMapper;
using SmartWealth.AccountService.Models;
using SmartWealth.AccountService.ViewModels;

namespace SmartWealth.AccountService.Utilities.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<AccountViewModel, Account>().ReverseMap();
    }
}