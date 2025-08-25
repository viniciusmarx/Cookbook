using Cookbook.Domain.Enums;

namespace Cookbook.Domain.Entities;

public class Recipe : EntityBase
{
    public required string Title { get; set; }
    public CookingTime? CookingTime { get; set; }
    public Difficulty? Difficulty { get; set; }
    public required IEnumerable<Ingredient> Ingredients { get; set; }
    public required IEnumerable<Instruction> Instructions { get; set; }
    public required IEnumerable<DishType> DishTypes { get; set; }
    public long UserId { get; set; }
}
