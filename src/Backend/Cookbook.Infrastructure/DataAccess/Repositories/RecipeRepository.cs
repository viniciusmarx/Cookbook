using Cookbook.Domain.Entities;
using Cookbook.Domain.Repositories.Recipe;

namespace Cookbook.Infrastructure.DataAccess.Repositories;

public class RecipeRepository(CookbookDbContext dbContext) : IRecipeRepository
{
    private readonly CookbookDbContext _dbContext = dbContext;

    public async Task Add(Recipe recipe) => await _dbContext.Recipes.AddAsync(recipe);
}
