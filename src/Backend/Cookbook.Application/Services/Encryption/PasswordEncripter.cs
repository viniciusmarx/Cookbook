using Cookbook.Communication.Settings;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;

namespace Cookbook.Application.Services.Encryption;

public class PasswordEncripter(IOptions<PasswordSettings> options)
{
    private readonly PasswordSettings _passwordSettings = options.Value;

    public string Encrypt(string password)
    {
        var newPassword = $"{password}{_passwordSettings.PasswordSalt}";

        var bytes = Encoding.UTF8.GetBytes(newPassword);
        var hashBytes = SHA512.HashData(bytes);

        return StringBytes(hashBytes);
    }

    private static string StringBytes(byte[] bytes)
    {
        var sb = new StringBuilder();
        foreach (byte b in bytes)
        {
            sb.Append(b.ToString("x2"));
        }
        return sb.ToString();
    }
}
