using Infrastructure.Configurations;
using Microsoft.Extensions.Options;

namespace WebDiplomaWork.OptionsSetup;

public class AzureStorageConfigSetup : IConfigureOptions<AzureStorageConfig>
{
    private const string SectionName = "AzureStorageConfig";
    private readonly IConfiguration _configuration;

    public AzureStorageConfigSetup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(AzureStorageConfig options)
    {
        _configuration.GetSection(SectionName).Bind(options);
    }
}