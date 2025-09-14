using System.ComponentModel.DataAnnotations.Schema;

namespace Cookbook.Domain.Entities;

[Table("Ingredients")]
public class Ingredient : EntityBase
{
    public required string Item { get; set; }
    public long RecipeId { get; set; }
}
