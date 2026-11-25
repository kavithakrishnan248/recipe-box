namespace RecipeBox.Api.Entities;

public class Ingredient
{
    public int Id { get; set; }

    public int RecipeId { get; set; }

    public required string Name { get; set; }

    public required string Quantity { get; set; }
}
