using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using SmartWealth.CategoryService.Models;
using SmartWealth.CategoryService.Repository;
using SmartWealth.CategoryService.ViewModels;
using SmartWealth.CategoryService.Utilities.Exceptions;
using CloudinaryDotNet.Actions;

namespace SmartWealth.CategoryService.Services;

public class CategoryService(IMapper mapper, IRepository<Category> repository, IValidator<CategoryViewModel> validator, ICloudinaryService cloudinary) : ICategoryService
{
    private readonly IMapper _mapper = mapper;
    private readonly IRepository<Category> _repository = repository;
    private readonly IValidator<CategoryViewModel> _validator = validator;
    private readonly ICloudinaryService _cloudinary = cloudinary;

    public async Task<List<Category>> GetCategoriesAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<Category> GetCategoryAsync(Guid id)
    {
        return await _repository.GetAsync(id);
    }

    public async Task CreateCategoryAsync(CategoryViewModel createdCategory)
    {
        ValidationResult validationResult = await _validator.ValidateAsync(createdCategory);
        if (validationResult.IsValid)
        {
            Category category = _mapper.Map<Category>(createdCategory);
            category.Id = Guid.NewGuid();

            if (createdCategory.Icon != null)
            {
                ImageUploadResult result = await _cloudinary.UploadPhotoAsync(createdCategory.Icon);
                category.IconUrl = result.Url.ToString();
            }

            await _repository.AddAsync(category);
        }
        else
        {
            throw new NotValidException(validationResult.Errors.Select(failure => failure.ErrorMessage).ToArray());
        }
    }

    public async Task EditCategoryAsync(Guid id, CategoryViewModel editedCategory)
    {
        ValidationResult validationResult = await _validator.ValidateAsync(editedCategory);
        if (validationResult.IsValid)
        {
            Category category = _mapper.Map<Category>(editedCategory);
            category.Id = id;

            if (editedCategory.Icon != null)
            {
                ImageUploadResult result = await _cloudinary.UploadPhotoAsync(editedCategory.Icon);
                category.IconUrl = result.Url.ToString();
            }

            await _repository.UpdateAsync(category);
        }
        else
        {
            throw new NotValidException(validationResult.Errors.Select(failure => failure.ErrorMessage).ToArray());
        }
    }

    public async Task DeleteCategoryAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }
}