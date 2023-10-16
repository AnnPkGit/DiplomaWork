using System.Text.Json.Serialization;
using Application.Common.Interfaces;
using WebDiplomaWork.OptionsSetup;
using WebDiplomaWork.Services;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddWebUIServices(
        this IServiceCollection services)
    {
        services.ConfigureOptions<JwtOptionsSetup>();
        services.ConfigureOptions<EmailOptionsSetup>();
        services.ConfigureOptions<AzureStorageConfigSetup>();
        
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        
        services.AddAuthentication()
            .AddScheme<SessionTokenAuthenticationSchemeOptions, SessionTokenAuthenticationSchemeHandler>(
                "SessionTokens",
                _ => {}
            );
        
        services.AddControllers()
            .AddJsonOptions(x => x.JsonSerializerOptions.MaxDepth = 20)
            .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
        
        return services;
    }
}