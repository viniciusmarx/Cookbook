using Bogus;
using Cookbook.Communication.Requests;
using Cookbook.Domain.Enums;

namespace CommomTestUtilities.Requests;

public class RecipeRequestBuilder
{
    public static RecipeRequest Build()
    {
        var step = 1;

        return new Faker<RecipeRequest>()
            .RuleFor(recipe => recipe.Title, f => f.Lorem.Word())
            .RuleFor(recipe => recipe.CookingTime, f => f.PickRandom<CookingTime>())
            .RuleFor(recipe => recipe.Difficulty, f => f.PickRandom<Difficulty>())
            .RuleFor(recipe => recipe.Ingredients, f => f.Make(3, () => f.Commerce.ProductName()))
            .RuleFor(recipe => recipe.DishTypes, f => f.Make(3, () => f.PickRandom<DishType>()))
            .RuleFor(recipe => recipe.Instructions, f => f.Make(3, () => new InstructionRequest { Step = step++, Text = f.Lorem.Paragraph() }));
    }
}
