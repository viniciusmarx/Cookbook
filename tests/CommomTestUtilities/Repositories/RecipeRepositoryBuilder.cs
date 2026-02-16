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

    public RecipeRepositoryBuilder GetById(User user, Recipe? recipe)
    {
        if (recipe is not null)
            _repository.Setup(repository => repository.GetById(user, recipe.Id)).ReturnsAsync(recipe);

        return this;
    }

    public IRecipeRepository Build() => _repository.Object;
}
