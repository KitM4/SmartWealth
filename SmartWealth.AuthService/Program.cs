using FluentValidation;
using SmartWealth.AuthService.Models;
using SmartWealth.AuthService.Services;
using SmartWealth.AuthService.Database;
using SmartWealth.AuthService.Utilities;
using SmartWealth.AuthService.ViewModels;
using SmartWealth.AuthService.Utilities.JWT;
using SmartWealth.AuthService.Utilities.Mappers;
using SmartWealth.AuthService.Services.Interfaces;
using SmartWealth.AuthService.Utilities.Validators;
using SmartWealth.AuthService.Utilities.Cloudinary;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DatabaseContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("Sqlite")));
builder.Services.AddIdentity<User, IdentityRole<Guid>>().AddEntityFrameworkStores<DatabaseContext>().AddDefaultTokenProviders();

builder.Services.AddScoped<IHttpService, HttpService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<ICloudinaryService, CloudinaryService>();

builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("ApiSettings:JwtOptions"));
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));
builder.Services.AddTransient<IValidator<UserLoginViewModel>, UserLoginViewModelValidator>();
builder.Services.AddTransient<IValidator<UserRegistrationViewModel>, UserRegistrationViewModelValidator>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();
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
app.UseStaticFiles();
app.MapControllers();

app.Run();