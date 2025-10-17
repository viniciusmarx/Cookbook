using Cookbook.Domain.Enums;

namespace Cookbook.Domain.ValueObjects;

public record RecipeFilters(string? RecipeTitle, IEnumerable<CookingTime> CookingTimes, IEnumerable<Difficulty> Difficulties, IEnumerable<DishType> DishTypes);
