namespace RecipeBox.Api.Entities;

public class Recipe
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public required string Instructions { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public List<Ingredient> Ingredients { get; set; } = [];
}
