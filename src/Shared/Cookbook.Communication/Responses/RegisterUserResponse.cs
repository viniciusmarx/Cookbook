namespace Cookbook.Communication.Responses;

public class RegisterUserResponse
{
    public string Name { get; set; } = string.Empty;
    public TokenResponse Tokens { get; set; } = default!;
}