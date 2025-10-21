namespace Cookbook.Domain.Repositories.Recipe;

using Cookbook.Domain.Entities;
using Cookbook.Domain.ValueObjects;

public interface IRecipeRepository
{
    Task Add(Entities.Recipe recipe);
    Task<IEnumerable<Recipe>> Filter(User user, RecipeFilters filters);
    Task<Recipe?> GetById(User user, long recipeId);
}
