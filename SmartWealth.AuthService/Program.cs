using FluentValidation;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
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
using Microsoft.AspNetCore.Authentication.JwtBearer;

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
builder.Services.AddTransient<IValidator<UserViewModel>, UserViewModelValidatior>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition(name: JwtBearerDefaults.AuthenticationScheme, securityScheme: new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter the Bearer Authorization string as following: `Bearer Generated-JWT-Token`",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference= new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id=JwtBearerDefaults.AuthenticationScheme
                }
            }, Array.Empty<string>()
        }
    });
});

builder.AddAppAuthetication();
builder.Services.AddAuthorization();
builder.Services.AddValidationErrorHandling();

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