using System;
using System.Security.Cryptography;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

public static class PasswordHelper
{
    public static (string Hash, string Salt) HashPassword(string password, int iterations = 100_000)
    {
        var saltBytes = new byte[16];
        using (var rng = RandomNumberGenerator.Create()) rng.GetBytes(saltBytes);
        var salt = Convert.ToBase64String(saltBytes);

        using (var derive = new Rfc2898DeriveBytes(password, saltBytes, iterations, HashAlgorithmName.SHA256))
        {
            var hash = Convert.ToBase64String(derive.GetBytes(32));
            return (hash, salt);
        }
    }

    public static bool VerifyPassword(string password, string storedHash, string storedSalt, int iterations = 100_000)
    {
        var saltBytes = Convert.FromBase64String(storedSalt);
        using (var derive = new Rfc2898DeriveBytes(password, saltBytes, iterations, HashAlgorithmName.SHA256))
        {
            var testHash = Convert.ToBase64String(derive.GetBytes(32));
            return testHash == storedHash;
        }
    }
}
