namespace App.Common.Interfaces;

public interface IHasher
{
    string Hash(string message);
    string GenerateSalt();
    string HashPassword(string password, string salt);
}