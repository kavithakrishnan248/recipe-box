namespace RecipeBox.Api.Dtos;

public record RecipeDto(
    int Id,
    string Name,
    string Instructions,
    DateTimeOffset CreatedAt,
    IReadOnlyList<IngredientDto> Ingredients);

public record IngredientDto(int Id, string Name, string Quantity);
