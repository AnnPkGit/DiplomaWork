using Infrastructure.Configurations;
using Microsoft.Extensions.Options;

namespace WebDiplomaWork.OptionsSetup;

public class SmsVerifyOptionsSetup : IConfigureOptions<SmsVerifyOptions>
{
    private const string SectionName = "SmsVerifyOptions";
    private readonly IConfiguration _configuration;
        
    public SmsVerifyOptionsSetup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(SmsVerifyOptions options)
    {
        _configuration.GetSection(SectionName).Bind(options);
    }
}