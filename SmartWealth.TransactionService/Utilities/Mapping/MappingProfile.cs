using AutoMapper;
using SmartWealth.TransactionService.Models;
using SmartWealth.TransactionService.ViewModels;

namespace SmartWealth.TransactionService.Utilities.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<TransactionViewModel, Transaction>().ReverseMap();
    }
}