using Cookbook.Domain.Enums;

namespace Cookbook.Communication.Requests;

public class FilterRecipeRequest
{
    public string? IngredientTitle { get; set; }
    public IList<CookingTime> CookingTimes { get; set; } = [];
    public IList<Difficulty> Difficulties { get; set; } = [];
    public IList<DishType> DishTypes { get; set; } = [];
}
