using Microsoft.EntityFrameworkCore;
using RecipeBox.Api.Data;
using RecipeBox.Api.Dtos;
using RecipeBox.Api.Entities;

namespace RecipeBox.Api.Services;

public class RecipeService(RecipeBoxDbContext db, TimeProvider timeProvider) : IRecipeService
{
    public async Task<PagedResult<RecipeListItemDto>> ListAsync(
        RecipeListQuery query, CancellationToken cancellationToken)
    {
        var recipes = db.Recipes.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var search = query.Search.Trim();
            recipes = recipes.Where(r => r.Name.Contains(search));
        }

        var totalCount = await recipes.CountAsync(cancellationToken);

        // Id breaks ties so rows never move between pages.
        var items = await recipes
            .OrderByDescending(r => r.CreatedAt)
            .ThenByDescending(r => r.Id)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(r => new RecipeListItemDto(r.Id, r.Name, r.CreatedAt))
            .ToListAsync(cancellationToken);

        return new PagedResult<RecipeListItemDto>(items, query.Page, query.PageSize, totalCount);
    }

    public Task<RecipeDto?> GetAsync(int id, CancellationToken cancellationToken) =>
        db.Recipes
            .AsNoTracking()
            .Where(r => r.Id == id)
            .Select(r => new RecipeDto(
                r.Id,
                r.Name,
                r.Instructions,
                r.CreatedAt,
                r.Ingredients
                    .OrderBy(i => i.Id)
                    .Select(i => new IngredientDto(i.Id, i.Name, i.Quantity))
                    .ToList()))
            .SingleOrDefaultAsync(cancellationToken);

    public async Task<RecipeDto> CreateAsync(SaveRecipeRequest request, CancellationToken cancellationToken)
    {
        var recipe = new Recipe
        {
            Name = request.Name,
            Instructions = request.Instructions,
            CreatedAt = timeProvider.GetUtcNow(),
            Ingredients = ToIngredients(request),
        };

        db.Recipes.Add(recipe);
        await db.SaveChangesAsync(cancellationToken);

        return new RecipeDto(
            recipe.Id,
            recipe.Name,
            recipe.Instructions,
            recipe.CreatedAt,
            recipe.Ingredients.Select(i => new IngredientDto(i.Id, i.Name, i.Quantity)).ToList());
    }

    public async Task<bool> UpdateAsync(int id, SaveRecipeRequest request, CancellationToken cancellationToken)
    {
        var recipe = await db.Recipes
            .Include(r => r.Ingredients)
            .SingleOrDefaultAsync(r => r.Id == id, cancellationToken);

        if (recipe is null)
        {
            return false;
        }

        recipe.Name = request.Name;
        recipe.Instructions = request.Instructions;
        db.Ingredients.RemoveRange(recipe.Ingredients);
        recipe.Ingredients = ToIngredients(request);

        await db.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        // Ingredients go with the recipe through the cascading foreign key.
        var deleted = await db.Recipes.Where(r => r.Id == id).ExecuteDeleteAsync(cancellationToken);
        return deleted > 0;
    }

    private static List<Ingredient> ToIngredients(SaveRecipeRequest request) =>
        request.Ingredients
            .Select(i => new Ingredient { Name = i.Name, Quantity = i.Quantity })
            .ToList();
}
