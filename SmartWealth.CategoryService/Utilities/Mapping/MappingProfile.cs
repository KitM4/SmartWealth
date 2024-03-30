using AutoMapper;
using SmartWealth.CategoryService.Models;
using SmartWealth.CategoryService.ViewModels;

namespace SmartWealth.CategoryService.Utilities.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CategoryViewModel, Category>().ReverseMap();
    }
}