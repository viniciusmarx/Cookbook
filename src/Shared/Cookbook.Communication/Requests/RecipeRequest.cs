using Cookbook.Domain.Enums;

namespace Cookbook.Communication.Requests;

public class RecipeRequest
{
    public required string Title { get; set; }
    public CookingTime? CookingTime { get; set; }
    public Difficulty? Difficulty { get; set; }
    public IEnumerable<string> Ingredients { get; set; } = [];
    public IEnumerable<InstructionRequest> Instructions { get; set; } = [];
    public IEnumerable<DishType> DishTypes { get; set; } = [];
}
