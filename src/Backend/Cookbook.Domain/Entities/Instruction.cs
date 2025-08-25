namespace Cookbook.Domain.Entities;

public class Instruction : EntityBase
{
    public int Step { get; set; }
    public required string Text { get; set; }
    public long RecipeId { get; set; }
}
