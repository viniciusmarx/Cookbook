using Cookbook.Communication.Responses;

namespace Cookbook.Application.UseCases.Recipe.GetById;

public interface IGetRecipeByIdUseCase
{
    Task<RecipeResponse> Execute(long recipeId);
}
