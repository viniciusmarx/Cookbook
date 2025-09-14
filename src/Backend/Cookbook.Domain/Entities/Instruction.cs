using System.ComponentModel.DataAnnotations.Schema;

namespace Cookbook.Domain.Entities;

[Table("Instructions")]
public class Instruction : EntityBase
{
    public int Step { get; set; }
    public required string Text { get; set; }
    public long RecipeId { get; set; }
}
