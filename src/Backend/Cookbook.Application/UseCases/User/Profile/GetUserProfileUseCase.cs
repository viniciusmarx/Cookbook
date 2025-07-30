using AutoMapper;
using Cookbook.Communication.Responses;
using Cookbook.Domain.Services.LoggedUser;

namespace Cookbook.Application.UseCases.User.Profile;

public class GetUserProfileUseCase(ILoggedUser loggedUser, IMapper mapper) : IGetUserProfileUseCase
{
    private ILoggedUser _loggedUser = loggedUser;
    private readonly IMapper _mapper = mapper;

    public async Task<UserProfileResponse> Execute()
    {
        var user = await _loggedUser.User();

        return _mapper.Map<UserProfileResponse>(user);
    }
}