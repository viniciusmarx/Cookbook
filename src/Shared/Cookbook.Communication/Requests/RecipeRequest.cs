using Cookbook.Domain.Enums;

namespace Cookbook.Communication.Requests;

public class RecipeRequest
{
    public required string Title { get; set; }
    public CookingTime? CookingTime { get; set; }
    public Difficulty? Difficulty { get; set; }
    public IList<string> Ingredients { get; set; } = [];
    public IList<InstructionRequest> Instructions { get; set; } = [];
    public IList<DishType> DishTypes { get; set; } = [];
}
