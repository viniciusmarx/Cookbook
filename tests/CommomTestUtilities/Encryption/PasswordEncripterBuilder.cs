using Cookbook.Application.Services.Encryption;
using Cookbook.Communication.Settings;
using Microsoft.Extensions.Options;

namespace CommomTestUtilities.Encryption;

public class PasswordEncripterBuilder
{
    public static PasswordEncripter Build() => new(Options.Create(new PasswordSettings { PasswordSalt = "saltTeste" }));
}
