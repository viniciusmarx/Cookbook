using Cookbook.Domain.Entities;
using Cookbook.Domain.Repositories.Recipe;
using Cookbook.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Cookbook.Infrastructure.DataAccess.Repositories;

public class RecipeRepository(CookbookDbContext dbContext) : IRecipeRepository
{
    private readonly CookbookDbContext _dbContext = dbContext;

    public async Task Add(Recipe recipe) => await _dbContext.Recipes.AddAsync(recipe);

    public async Task<IEnumerable<Recipe>> Filter(User user, RecipeFilters filters)
    {
        var query = _dbContext.Recipes.AsNoTracking().Include(recipe => recipe.Ingredients).Where(recipe => recipe.IsActive && recipe.UserId == user.Id);

        if (filters.Difficulties.Any())
            query = query.Where(recipe => recipe.Difficulty.HasValue && filters.Difficulties.Contains(recipe.Difficulty.Value));

        if (filters.CookingTimes.Any())
            query = query.Where(recipe => recipe.CookingTime.HasValue && filters.CookingTimes.Contains(recipe.CookingTime.Value));

        if (filters.DishTypes.Any())
            query = query.Where(recipe => recipe.DishTypes.Any(dishType => filters.DishTypes.Contains(dishType.Type)));

        if (filters.RecipeTitle is not null)
            query = query.Where(recipe => recipe.Title.Contains(filters.RecipeTitle) || recipe.Ingredients.Any(ingredient => ingredient.Item.Contains(filters.RecipeTitle)));

        return await query.ToListAsync();
    }
}
