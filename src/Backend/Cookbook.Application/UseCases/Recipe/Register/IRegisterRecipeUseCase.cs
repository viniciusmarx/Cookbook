using Cookbook.Communication.Requests;
using Cookbook.Communication.Responses;

namespace Cookbook.Application.UseCases.Recipe.Register;

public interface IRegisterRecipeUseCase
{
    public Task<RegisteredRecipeResponse> Execute(RecipeRequest request);
}
