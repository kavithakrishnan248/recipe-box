using System.ComponentModel.DataAnnotations;

namespace RecipeBox.Api.Dtos;

public record RecipeListQuery
{
    [MaxLength(200)]
    public string? Search { get; init; }

    [Range(1, 100_000)]
    public int Page { get; init; } = 1;

    [Range(1, 100)]
    public int PageSize { get; init; } = 20;
}
