using Cookbook.Domain.Entities;
using Cookbook.Domain.Repositories.Recipe;
using Cookbook.Domain.ValueObjects;
using Moq;

namespace CommomTestUtilities.Repositories;

public class RecipeRepositoryBuilder
{
    private readonly Mock<IRecipeRepository> _repository = new();

    public RecipeRepositoryBuilder Filter(User user, IList<Recipe> recipes)
    {
        _repository.Setup(r => r.Filter(user, It.IsAny<RecipeFilters>())).ReturnsAsync(recipes);

        return this;
    }

    public IRecipeRepository Build()
    {
        return _repository.Object;
    }
}
