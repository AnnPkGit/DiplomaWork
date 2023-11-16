using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Infrastructure.Configurations;

public class SmsVerifyOptions
{
    public string BaseUrl { get; set; }
    public string AuthToken { get; set; }
    
}