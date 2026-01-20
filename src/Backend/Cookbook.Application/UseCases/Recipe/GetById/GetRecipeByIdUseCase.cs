using AutoMapper;
using Cookbook.Communication.Responses;
using Cookbook.Domain.Repositories.Recipe;
using Cookbook.Domain.Services.LoggedUser;
using Cookbook.Exceptions;
using Cookbook.Exceptions.ExceptionsBase;

namespace Cookbook.Application.UseCases.Recipe.GetById;

public class GetRecipeByIdUseCase(IMapper mapper, ILoggedUser loggedUser, IRecipeRepository recipeRepository) : IGetRecipeByIdUseCase
{
    private readonly IMapper _mapper = mapper;
    private readonly ILoggedUser _loggedUser = loggedUser;
    private readonly IRecipeRepository _recipeRepository = recipeRepository;

    public async Task<RecipeResponse> Execute(long recipeId)
    {
        var loggedUser = await _loggedUser.User();

        var recipe = await _recipeRepository.GetById(loggedUser, recipeId);

        return recipe is null
            ? throw new NotFoundException(ResourceMessagesException.RECIPE_NOT_FOUND)
            : _mapper.Map<RecipeResponse>(recipe);
    }
}
