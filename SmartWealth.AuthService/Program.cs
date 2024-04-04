using FluentValidation;
using SmartWealth.AuthService.Database;
using SmartWealth.AuthService.ViewModels;
using SmartWealth.AuthService.Utilities.JWT;
using SmartWealth.AuthService.Utilities.Mappers;
using SmartWealth.AuthService.Utilities.Validators;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DatabaseContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("Sqlite")));

builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("ApiSettings:JwtOptions"));
builder.Services.AddTransient<IValidator<UserLoginViewModel>, UserLoginViewModelValidator>();
builder.Services.AddTransient<IValidator<UserRegistrationViewModel>, UserRegistrationViewModelValidator>();

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