using Application.Common.Interfaces;
using WebDiplomaWork.OptionsSetup;
using WebDiplomaWork.Services;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static void AddWebUIServices(this IServiceCollection services)
    {
        services.ConfigureOptions<JwtOptionsSetup>();
        services.ConfigureOptions<EmailOptionsSetup>();
        
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        
        services.AddAuthentication()
            .AddScheme<SessionTokenAuthenticationSchemeOptions, SessionTokenAuthenticationSchemeHandler>(
                "SessionTokens",
                _ => {}
            );
    }
}