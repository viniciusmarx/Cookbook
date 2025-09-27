using Cookbook.Communication.Requests;
using Cookbook.Communication.Responses;

namespace Cookbook.Application.UseCases.Recipe.Filter;

public interface IFilterRecipeUseCase
{
    Task<IEnumerable<RecipesResponse>> Execute(FilterRecipeRequest request);
}
