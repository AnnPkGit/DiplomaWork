using Application.Common.Interfaces;
using Infrastructure.Configurations;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
        services.ConfigureOptions<SmsVerifyOptionsSetup>();
        services.ConfigureOptions<AzureStorageConfigSetup>();
        
        services.AddSingleton<ICurrentUserService, CurrentUserService>();
        
        services.AddAuthentication()
            .AddScheme<SessionTokenAuthenticationSchemeOptions, SessionTokenAuthenticationSchemeHandler>(
                "SessionTokens",
                _ => {}
            );
        
        return services;
    }
}