namespace Cookbook.Domain.Entities;

public class Ingredient : EntityBase
{
    public required string Item { get; set; }
    public long RecipeId { get; set; }
}
