using AutoMapper;
using Cookbook.Communication.Requests;
using Cookbook.Communication.Responses;
using Cookbook.Domain.Enums;
using Sqids;

namespace Cookbook.Application.Services.AutoMapper;

public class AutoMapping : Profile
{
    private readonly SqidsEncoder<long> _idEncoder;
    public AutoMapping(SqidsEncoder<long> idEncoder)
    {
        _idEncoder = idEncoder;
        RequestToDomain();
        DomainToResponse();
    }

    private void RequestToDomain()
    {
        CreateMap<RegisterUserRequest, Domain.Entities.User>()
            .ForMember(dest => dest.Password, opt => opt.Ignore());

        CreateMap<RecipeRequest, Domain.Entities.Recipe>()
            .ForMember(dest => dest.Instructions, opt => opt.Ignore())
            .ForMember(dest => dest.Ingredients, opt => opt.MapFrom(source => source.Ingredients.Distinct()))
            .ForMember(dest => dest.DishTypes, opt => opt.MapFrom(source => source.DishTypes.Distinct()));

        CreateMap<string, Domain.Entities.Ingredient>()
            .ForMember(dest => dest.Item, opt => opt.MapFrom(source => source));

        CreateMap<DishType, Domain.Entities.DishType>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(source => source));

        CreateMap<InstructionRequest, Domain.Entities.Instruction>();
    }

    private void DomainToResponse()
    {
        CreateMap<Domain.Entities.User, UserProfileResponse>();

        CreateMap<Domain.Entities.Recipe, RegisteredRecipeResponse>()
            .ForMember(dest => dest.Id, config => config.MapFrom(source => _idEncoder.Encode(source.Id)));

        CreateMap<Domain.Entities.Recipe, RecipesResponse>()
            .ForMember(dest => dest.Id, config => config.MapFrom(source => _idEncoder.Encode(source.Id)))
            .ForMember(dest => dest.AmountIngredients, config => config.MapFrom(source => source.Ingredients.Count()));
    }
}
