using System.Reflection;
using Application.Common.Behaviours;
using Application.Common.Interfaces;
using Application.Common.Notification;
using Application.Common.Services;
using FluentValidation;
using MediatR;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMediatR(cfg => {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehaviour<,>));
        });
        
        services.AddScoped<IEmailConfirmationSender, EmailConfirmationSender>();
        services.AddScoped<IMediaService, MediaService>();
        services.AddScoped<IMuteNotificationOptionsChecker, MuteNotificationOptionsChecker>();
        
        return services;
    }
}