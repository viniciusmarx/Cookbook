namespace Cookbook.Domain.Security.Tokens;

public interface IAccessTokenGenerator
{
    public string GenerateToken(Guid userIdentifier);
}