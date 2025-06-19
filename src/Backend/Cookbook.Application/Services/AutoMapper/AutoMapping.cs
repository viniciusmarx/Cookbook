using AutoMapper;
using Cookbook.Communication.Requests;

namespace Cookbook.Application.Services.AutoMapper;

internal class AutoMapping : Profile
{
    public AutoMapping()
    {
        RequestToDomain();
    }

    private void RequestToDomain()
    {
        CreateMap<RegisterUserRequest, Domain.Entities.User>()
            .ForMember(dest => dest.Password, opt => opt.Ignore());
    }
}
