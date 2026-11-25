using RecipeBox.Api.Dtos;

namespace RecipeBox.Api.Services;

public interface IRecipeService
{
    Task<PagedResult<RecipeListItemDto>> ListAsync(RecipeListQuery query, CancellationToken cancellationToken);

    /// <returns>The recipe, or null if it does not exist.</returns>
    Task<RecipeDto?> GetAsync(int id, CancellationToken cancellationToken);

    Task<RecipeDto> CreateAsync(SaveRecipeRequest request, CancellationToken cancellationToken);

    /// <summary>Replaces the recipe, including its whole ingredient list.</summary>
    /// <returns>False if the recipe does not exist.</returns>
    Task<bool> UpdateAsync(int id, SaveRecipeRequest request, CancellationToken cancellationToken);

    /// <returns>False if the recipe does not exist.</returns>
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);
}
