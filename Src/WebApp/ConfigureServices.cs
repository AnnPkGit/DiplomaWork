using Application.Common.Interfaces;
using WebApp.OptionsSetup;
using WebApp.Services;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddWebUIServices(
        this IServiceCollection services)
    {
        services.ConfigureOptions<JwtOptionsSetup>();
        services.ConfigureOptions<EmailOptionsSetup>();
        services.ConfigureOptions<SmsVerifyOptionsSetup>();
        services.ConfigureOptions<AzureStorageConfigSetup>();
        services.ConfigureOptions<EmailConfirmationSenderOptionsSetup>();
        
        services.AddSingleton<ICurrentUserService, CurrentUserService>();
        
        services.AddAuthentication()
            .AddScheme<SessionTokenAuthenticationSchemeOptions, SessionTokenAuthenticationSchemeHandler>(
                "SessionTokens",
                _ => {}
            );
        
        return services;
    }
}