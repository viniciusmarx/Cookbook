using AutoMapper;
using Cookbook.Communication.Requests;
using Cookbook.Communication.Responses;

namespace Cookbook.Application.Services.AutoMapper;

public class AutoMapping : Profile
{
    public AutoMapping()
    {
        RequestToDomain();
        DomainToResponse();
    }

    private void RequestToDomain()
    {
        CreateMap<RegisterUserRequest, Domain.Entities.User>()
            .ForMember(dest => dest.Password, opt => opt.Ignore());
    }

    private void DomainToResponse()
    {
        CreateMap<Domain.Entities.User, UserProfileResponse>();
    }
}
