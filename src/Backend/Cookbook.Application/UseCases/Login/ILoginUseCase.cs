using Cookbook.Communication.Requests;
using Cookbook.Communication.Responses;

namespace Cookbook.Application.UseCases.Login;

public interface ILoginUseCase
{
    public Task<RegisterUserResponse> Execute(LoginRequest request);
}
