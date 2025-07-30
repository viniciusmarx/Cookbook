using Cookbook.Communication.Requests;

namespace Cookbook.Application.UseCases.User.ChangePassword;

public interface IChangePasswordUseCase
{
    public Task Execute(ChangePasswordRequest request);
}