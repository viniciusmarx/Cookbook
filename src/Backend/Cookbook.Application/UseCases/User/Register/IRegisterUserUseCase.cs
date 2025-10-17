using Cookbook.Communication.Requests;
using Cookbook.Communication.Responses;

namespace Cookbook.Application.UseCases.User.Register;

public interface IRegisterUserUseCase
{
    public Task<RegisterUserResponse> Execute(RegisterUserRequest request);
}
