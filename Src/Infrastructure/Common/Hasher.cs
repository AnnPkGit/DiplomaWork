using System.Security.Cryptography;
using System.Text;
using Application.Common.Interfaces;
using Konscious.Security.Cryptography;

namespace Infrastructure.Common;

public class Hasher : IHasher
{
    public string Hash(string message)
    {
        using (var algo =
               System.Security.Cryptography.SHA1.Create())
        {
            byte[] hash = algo.ComputeHash(
                System.Text.Encoding.UTF8.GetBytes(message));
            var sb = new System.Text.StringBuilder();
            foreach (byte b in hash)
            {
                sb.Append(b.ToString("X02"));
            }
            return sb.ToString();
        }
    }
    public string GenerateSalt()
    {
        byte[] saltBytes = new byte[32];
        using (var rng = new RNGCryptoServiceProvider())
        {
            rng.GetBytes(saltBytes);
        }
        return Convert.ToBase64String(saltBytes);
    }

    // метод для хеширования пароля
    public string HashPassword(string password, string salt)
    {
        var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
        {
            Salt = Encoding.UTF8.GetBytes(salt),
            DegreeOfParallelism = 8, 
            MemorySize = 65536,      
            Iterations = 4          
        };

        byte[] hashBytes = argon2.GetBytes(32); // Итоговый хеш
        return Convert.ToBase64String(hashBytes);
    }
}
