using FluentValidation;
using SmartWealth.AccountService.Models;
using SmartWealth.AccountService.Services;
using SmartWealth.AccountService.Database;
using SmartWealth.AccountService.Utilities;
using SmartWealth.AccountService.ViewModels;
using SmartWealth.AccountService.Repositories;
using SmartWealth.AccountService.Utilities.Mapping;
using SmartWealth.AccountService.Services.Interfaces;
using SmartWealth.AccountService.Utilities.Validators;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DatabaseContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("Sqlite")));
builder.Services.AddScoped<IRepository<Account>, AccountRepository>();

builder.Services.AddScoped<IAccountService, AccountService>();

builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddTransient<IValidator<AccountViewModel>, AccountViewModelValidator>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.AddAppAuthetication();
builder.Services.AddAuthorization();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    if (args.Length > 0 && args.Contains("db-rebuild"))
        DatabaseInitializer.RebuildDatabase(app.Services.CreateScope());

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();