using SmartWealth.CategoryService.Models;
using SmartWealth.CategoryService.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SmartWealth.CategoryService.Database.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder
            .HasKey(category => category.Id);

        builder
            .Property(category => category.Name)
            .HasMaxLength(Constants.MaxCategoryNameLength)
            .IsRequired();

        builder
            .Property(category => category.Description)
            .HasMaxLength(Constants.MaxCategoryDescriptionLength);
    }
}