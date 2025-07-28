using Cookbook.Communication.Responses;

namespace Cookbook.Application.UseCases.User.Profile;

public interface IGetUserProfileUseCase
{
    public Task<UserProfileResponse> Execute();
}
