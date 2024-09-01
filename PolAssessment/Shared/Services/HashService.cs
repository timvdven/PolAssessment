using System.Security.Cryptography;
using System.Text;

namespace PolAssessment.Shared.Services;

public interface IHashService
{
    string GetHash(string input);
}

public class HashService : IHashService
{
    public string GetHash(string input)
    {
        using var sha256 = SHA256.Create() ?? throw new CryptographicException("Could not create SHA256 hash.");
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
        return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
    }
}
