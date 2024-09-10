using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace PolAssessment.Common.Lib.Services;

public interface IHashService
{
    string GetHash(string input);

    string GetHash(object jsonSerializibleObject);
}

public class HashService : IHashService
{
    public string GetHash(string input)
    {
        using var sha256 = SHA256.Create() ?? throw new CryptographicException("Could not create SHA256 hash.");
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
        return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
    }

    public string GetHash(object jsonSerializibleObject)
    {
        var json = JsonSerializer.Serialize(jsonSerializibleObject);
        return GetHash(json);
    }
}
