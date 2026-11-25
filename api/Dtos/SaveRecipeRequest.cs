using System.ComponentModel.DataAnnotations;

namespace RecipeBox.Api.Dtos;

/// <summary>Body for both POST and PUT.</summary>
public record SaveRecipeRequest(
    [Required, MaxLength(200)] string Name,
    [Required, MaxLength(20_000)] string Instructions,
    [Required, MaxLength(100)] IReadOnlyList<IngredientRequest> Ingredients) : IValidatableObject
{
    // Annotations validate each ingredient but do not reject null entries such as [null].
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Ingredients.Any(i => i is null))
        {
            yield return new ValidationResult("Ingredients must not contain null entries.", [nameof(Ingredients)]);
        }
    }
}

public record IngredientRequest(
    [Required, MaxLength(200)] string Name,
    [Required, MaxLength(100)] string Quantity);
