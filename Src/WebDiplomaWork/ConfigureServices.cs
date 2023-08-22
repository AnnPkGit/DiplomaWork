using App.Common.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using WebDiplomaWork.OptionsSetup;
using WebDiplomaWork.Services;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddWebUIServices(
        this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(Program).Assembly);
        
        // Jwt Configuration
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer();
        services.ConfigureOptions<JwtOptionsSetup>();
        services.ConfigureOptions<JwtBearerOptionsSetup>();
        
        // Other
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        
        return services;
    }
}