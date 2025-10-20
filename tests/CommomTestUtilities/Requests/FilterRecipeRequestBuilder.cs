using Bogus;
using Cookbook.Communication.Requests;
using Cookbook.Domain.Enums;

namespace CommomTestUtilities.Requests;

public class FilterRecipeRequestBuilder
{
    public static FilterRecipeRequest Build()
    {
        return new Faker<FilterRecipeRequest>()
            .RuleFor(r => r.CookingTimes, faker => faker.Make(1, () => faker.PickRandom<CookingTime>()))
            .RuleFor(r => r.Difficulties, faker => faker.Make(1, () => faker.PickRandom<Difficulty>()))
            .RuleFor(r => r.DishTypes, faker => faker.Make(1, () => faker.PickRandom<DishType>()))
            .RuleFor(r => r.IngredientTitle, faker => faker.Lorem.Word());
    }
}
