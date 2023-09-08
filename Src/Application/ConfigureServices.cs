using System.Reflection;
using Application.Common.Interfaces.Services;
using Application.Services;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services)
    {
        services.AddMediatR(cfg => {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });
        
        // Services
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IExampleService, ExampleService>();
        
        return services;
    }
}