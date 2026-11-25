using Microsoft.EntityFrameworkCore;
using RecipeBox.Api.Entities;

namespace RecipeBox.Api.Data;

public class RecipeBoxDbContext(DbContextOptions<RecipeBoxDbContext> options) : DbContext(options)
{
    public DbSet<Recipe> Recipes => Set<Recipe>();

    public DbSet<Ingredient> Ingredients => Set<Ingredient>();

    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(RecipeBoxDbContext).Assembly);
}
