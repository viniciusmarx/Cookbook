using Cookbook.Domain.Security.Tokens;
using Cookbook.Infrastructure.Security.Tokens;

namespace CommomTestUtilities.Tokens;

public class JwtTokenGeneratorBuilder
{
    public static IAccessTokenGenerator Build() => new JwtTokenGenerator(expirationTimeMinutes: 5, signingKey: "9CNKIUjXNojzdjUrzhqq7Yb6sJUSTvXl");
}