using FluentValidation;
using SmartWealth.CategoryService.Models;
using SmartWealth.CategoryService.Database;
using SmartWealth.CategoryService.ViewModels;
using SmartWealth.CategoryService.Repository;
using SmartWealth.CategoryService.Services;
using SmartWealth.CategoryService.Utilities.Mapping;
using SmartWealth.CategoryService.Utilities.Validators;
using SmartWealth.CategoryService.Utilities.Cloudinary;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DatabaseContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("Sqlite")));
builder.Services.AddScoped<IRepository<Category>, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICloudinaryService, CloudinaryService>();

builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddTransient<IValidator<CategoryViewModel>, CategoryViewModelValidator>();
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    if (args.Length > 0 && args.Contains("db-rebuild"))
        DatabaseInitializer.RebuildDatabase(app.Services.CreateScope());

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();