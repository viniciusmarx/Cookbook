namespace Cookbook.Domain.Repositories.Recipe;

public interface IRecipeRepository
{
    Task Add(Entities.Recipe recipe);
}
