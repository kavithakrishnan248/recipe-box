using Microsoft.AspNetCore.Mvc;
using RecipeBox.Api.Dtos;
using RecipeBox.Api.Services;

namespace RecipeBox.Api.Controllers;

[ApiController]
[Route("api/recipes")]
public class RecipesController(IRecipeService recipes) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PagedResult<RecipeListItemDto>>> List(
        [FromQuery] RecipeListQuery query, CancellationToken cancellationToken) =>
        await recipes.ListAsync(query, cancellationToken);

    [HttpGet("{id:int}")]
    public async Task<ActionResult<RecipeDto>> Get(int id, CancellationToken cancellationToken)
    {
        var recipe = await recipes.GetAsync(id, cancellationToken);
        return recipe is null ? NotFound() : recipe;
    }

    [HttpPost]
    public async Task<ActionResult<RecipeDto>> Create(
        SaveRecipeRequest request, CancellationToken cancellationToken)
    {
        var recipe = await recipes.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(Get), new { id = recipe.Id }, recipe);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id, SaveRecipeRequest request, CancellationToken cancellationToken)
    {
        var updated = await recipes.UpdateAsync(id, request, cancellationToken);
        return updated ? NoContent() : NotFound();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var deleted = await recipes.DeleteAsync(id, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }
}
