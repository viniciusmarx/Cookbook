using Cookbook.Domain.Enums;

namespace Cookbook.Communication.Requests;

public class FilterRecipeRequest
{
    public string? IngredientTitle { get; set; }
    public IEnumerable<CookingTime>? CookingTimes { get; set; }
    public IEnumerable<Difficulty>? Difficulties { get; set; }
    public IEnumerable<DishType>? DishTypes { get; set; }
}
