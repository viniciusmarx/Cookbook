using Cookbook.Application.Services.Encryption;
using Cookbook.Communication.Requests;
using Cookbook.Communication.Responses;
using Cookbook.Domain.Interfaces.Repositories.User;
using Cookbook.Exceptions.ExceptionsBase;

namespace Cookbook.Application.UseCases.Login;

public class LoginUseCase(IUserReadOnlyRepository repository, PasswordEncripter passwordEncripter) : ILoginUseCase
{
    private readonly IUserReadOnlyRepository _repository = repository;
    private readonly PasswordEncripter _passwordEncripter = passwordEncripter;

    public async Task<RegisterUserResponse> Execute(LoginRequest request)
    {
        var user = await _repository.GetByEmailAndPasswsord(request.Email, _passwordEncripter.Encrypt(request.Password)) ?? throw new InvalidLoginException();

        return new RegisterUserResponse
        {
            Name = user.Name
        };
    }
}
