using Application.Common.Interfaces;
using Infrastructure.Authentication;
using Infrastructure.Common;
using Infrastructure.Configuration;
using Infrastructure.Configuration.ConfigurationManager;
using Infrastructure.Configuration.Provider;
using Infrastructure.DbAccess;
using Microsoft.Extensions.Configuration;
using LocalConfigurationManager = Infrastructure.Configuration.ConfigurationManager.ConfigurationManager;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Configuration
        services.AddScoped<IConfigurationManager, LocalConfigurationManager>();
        services.Configure<GeneralConfiguration>(
            configuration.GetSection("GeneralConfiguration"));
        
        // DbAccess
        services.AddDbContext<IApplicationDbContext, ApplicationDbContext>();
        services.AddScoped<IDbAccessProvider, MariaDbAccessProvider>();
        
        // Other
        services.AddScoped<IHasher, Hasher>();
        services.AddScoped<ITokenProvider, TokenProvider>();
        
        return services;
    }
}