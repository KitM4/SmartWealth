using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace SmartWealth.AccountService.Utilities;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder AddAppAuthetication(this WebApplicationBuilder builder)
    {
        IConfigurationSection settingsSection = builder.Configuration.GetSection("ApiSettings");

        string secret = settingsSection.GetValue<string>("Secret")!;
        string issuer = settingsSection.GetValue<string>("Issuer")!;
        string audience = settingsSection.GetValue<string>("Audience")!;

        byte[] key = Encoding.ASCII.GetBytes(secret);

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = issuer,
                ValidAudience = audience,
                ValidateAudience = true
            };
        });

        return builder;
    }

    public static void AddValidationErrorHandling(this IServiceCollection services)
    {
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                List<string> errors = context.ModelState
                    .Where(e => e.Value != null && e.Value.Errors.Count > 0)
                    .SelectMany(e => e.Value!.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return new BadRequestObjectResult(new { error = string.Join(". ", errors) });
            };
        });
    }
}