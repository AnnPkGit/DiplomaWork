using Application.Common.Interfaces;
using Application.Common.Services;
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
    public static void AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        //....DB....
        if (configuration.GetValue<bool>("UseInMemoryDatabase"))
        {
            throw new NotImplementedException();
        }
        else if(configuration.GetValue<bool>("UseTestDatabase"))
        {
            services.AddDbContext<IApplicationDbContext, ApplicationDbContext>(options =>
            {
                var connectionString = configuration.GetConnectionString("TestDbConnection");
                var serverVersion = new MariaDbServerVersion(configuration.GetValue<string>("TestDbVersion"));
                options.UseMySql(connectionString, serverVersion);
            });
        }
        else
        {
            services.AddDbContext<IApplicationDbContext, ApplicationDbContext>(options =>
            {
                var connectionString = configuration.GetConnectionString("MariaDbConnection");
                var serverVersion = new MariaDbServerVersion(configuration.GetValue<string>("MariaDbVersion"));
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
        services.AddTransient<IDateTime, DateTimeService>();
        services.AddScoped<IMediaService, MediaService>();
        services.AddScoped<ISmsVerify, SmsVerify>();
        services.AddScoped<IFourDigitCodeGenerator, FourDigitCodeGeneratorService>();
        services.AddTransient<IConfirmPhoneService, ConfirmPhoneService>();
        services.AddScoped<IMediaStorage, MediaStorage>();
    }
}