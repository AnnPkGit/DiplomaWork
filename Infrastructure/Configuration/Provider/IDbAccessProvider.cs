using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Configuration.Provider;

public interface IDbAccessProvider
{
    string GetConnectionString();
    ServerVersion GetServerVersion();
}