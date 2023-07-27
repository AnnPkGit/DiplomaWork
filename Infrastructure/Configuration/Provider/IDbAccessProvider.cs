namespace Infrastructure.Configuration.Provider;

public interface IDbAccessProvider
{
    string GetConnectionString();
}