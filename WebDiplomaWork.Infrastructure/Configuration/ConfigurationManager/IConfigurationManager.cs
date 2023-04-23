namespace WebDiplomaWork.Infrastructure.Configuration.ConfigurationManager
{
    public interface IConfigurationManager
    {
        SshConfiguration SshConfiguration { get; }

        DbConfiguration DbConfiguration { get; }
    }
}
