using Cookbook.Communication.Requests;

namespace Cookbook.Application.UseCases.User.Update;

public interface IUpdateUserUseCase
{
    public Task Execute(UpdateUserRequest request);
}