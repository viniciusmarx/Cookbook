using Cookbook.Application.Services.Encryption;
using Cookbook.Communication.Requests;
using Cookbook.Communication.Responses;
using Cookbook.Domain.Repositories.User;
using Cookbook.Domain.Security.Tokens;
using Cookbook.Exceptions.ExceptionsBase;

namespace Cookbook.Application.UseCases.Login;

public class LoginUseCase(IUserReadOnlyRepository repository, PasswordEncripter passwordEncripter, IAccessTokenGenerator accessTokenGenerator) : ILoginUseCase
{
    private readonly IUserReadOnlyRepository _repository = repository;
    private readonly PasswordEncripter _passwordEncripter = passwordEncripter;
    private readonly IAccessTokenGenerator _accessTokenGenerator = accessTokenGenerator;

    public async Task<RegisterUserResponse> Execute(LoginRequest request)
    {
        var user = await _repository.GetByEmailAndPasswsord(request.Email, _passwordEncripter.Encrypt(request.Password)) ?? throw new InvalidLoginException();

        return new RegisterUserResponse
        {
            Name = user.Name,
            Tokens = new TokenResponse
            {
                AccessToken = _accessTokenGenerator.GenerateToken(user.UserIdentifier),
            }
        };
    }
}
