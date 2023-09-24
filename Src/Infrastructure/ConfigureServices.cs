using Application.Common.Interfaces;
using Infrastructure.Authentication;
using Infrastructure.Common;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Interceptors;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        if (configuration.GetValue<bool>("UseInMemoryDatabase"))
        {
            throw new NotImplementedException();
        }
        else
        {
            services.AddDbContext<IApplicationDbContext, ApplicationDbContext>(options =>
            {
                var connectionString = configuration.GetConnectionString("MariaDbConnection");
                var serverVersion = new MariaDbServerVersion("10.11.4");
                options.UseMySql(connectionString, serverVersion);
            });
        }
        
        // Other
        services.AddScoped<IHasher, Hasher>();
        services.AddScoped<ITokenProvider, TokenProvider>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<TokenValidationParameters, EmailVerifyTokenValidationParameters>();
        services.AddScoped<ITokenValidator, TokenValidator>();
        services.AddScoped<AuditableEntitySaveChangesInterceptor>();
        services.AddScoped<LegalEntitySaveChangesInterceptor>();
        services.AddTransient<IDateTime, DateTimeService>();
        
        return services;
    }
}