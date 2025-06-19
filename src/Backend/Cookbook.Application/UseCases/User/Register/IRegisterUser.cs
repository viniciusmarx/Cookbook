using Cookbook.Communication.Requests;
using Cookbook.Communication.Responses;

namespace Cookbook.Application.UseCases.User.Register;

public interface IRegisterUser
{
    public Task<RegisterUserResponse> Execute(RegisterUserRequest request);
}
