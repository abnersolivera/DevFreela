using System.Text;
using DevFreela.Core.Repositories;
using DevFreela.Infrastructure.Auth;
using DevFreela.Infrastructure.Notifications;
using DevFreela.Infrastructure.Persistence;
using DevFreela.Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SendGrid.Extensions.DependencyInjection;

namespace DevFreela.Infrastructure;

public static class InfrastructureModule
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddData(configuration)
            .AddRepositories()
            .AddAuth(configuration)
            .AddEmailService(configuration);
        return services;
    }

    private static IServiceCollection AddData(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DevFreelaDbContext>(o =>
            o.UseSqlServer(configuration.GetConnectionString("Database")));

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IProjectRepository, ProjectRepository>();
        return services;
    }
    
    private static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IAuthService, AuthService>();
        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["Jwt:Key"] ?? string.Empty))
                    
                };
            });
        return services;
    }

    private static IServiceCollection AddEmailService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSendGrid(options =>
        {
            options.ApiKey = configuration["SendGrid:ApiKey"];
        });
        services.AddScoped<IEmailService, EmailService>();
        return services;
    }
}