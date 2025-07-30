using Cookbook.Communication.Settings;
using Cookbook.Domain.Security.Cryptography;
using Cookbook.Infrastructure.Security.Cryptography;
using Microsoft.Extensions.Options;

namespace CommomTestUtilities.Encryption;

public class PasswordEncripterBuilder
{
    public static IPasswordEncripter Build() => new Sha512Encripter(Options.Create(new PasswordSettings { PasswordSalt = "saltTeste" }));
}