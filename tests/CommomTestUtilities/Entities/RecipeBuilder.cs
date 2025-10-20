using Bogus;
using Cookbook.Domain.Entities;
using Cookbook.Domain.Enums;

namespace CommomTestUtilities.Entities;

public class RecipeBuilder
{
    private static int _nextId = 1;

    public static IList<Recipe> GenerateRecipes(User user, int quantityRecipes = 2)
    {
        if (quantityRecipes < 1)
            quantityRecipes = 1;

        var faker = CreateRecipe(user);

        return faker.Generate(quantityRecipes);
    }

    private static Faker<Recipe> CreateRecipe(User user)
    {
        return new Faker<Recipe>()
            .RuleFor(r => r.Id, () => _nextId++)
            .RuleFor(r => r.Title, (f) => f.Lorem.Word())
            .RuleFor(r => r.CookingTime, (f) => f.PickRandom<CookingTime>())
            .RuleFor(r => r.Difficulty, (f) => f.PickRandom<Difficulty>())
            .RuleFor(r => r.Ingredients, (f) => f.Make(1, () => new Ingredient { Id = 1, Item = f.Commerce.ProductName() }))
            .RuleFor(r => r.Instructions, (f) => f.Make(1, () => new Instruction { Id = 1, Step = 1, Text = f.Lorem.Paragraph() }))
            .RuleFor(r => r.DishTypes, (f) => f.Make(1, () => new Cookbook.Domain.Entities.DishType { Id = 1, Type = f.PickRandom<Cookbook.Domain.Enums.DishType>() }))
            .RuleFor(r => r.UserId, () => user.Id);
    }
}
