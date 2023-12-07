using Application.Common.Configurations.Options;
using Microsoft.Extensions.Options;

namespace WebApp.OptionsSetup;

public class EmailConfirmationSenderOptionsSetup : IConfigureOptions<EmailConfirmationSenderOptions>
{
    private const string SectionName = "EmailConfirmationSenderOptions";
    private readonly IConfiguration _configuration;

    public EmailConfirmationSenderOptionsSetup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(EmailConfirmationSenderOptions options)
    {
        _configuration.GetSection(SectionName).Bind(options);
    }
}