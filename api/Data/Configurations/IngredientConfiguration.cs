using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecipeBox.Api.Entities;

namespace RecipeBox.Api.Data.Configurations;

public class IngredientConfiguration : IEntityTypeConfiguration<Ingredient>
{
    public void Configure(EntityTypeBuilder<Ingredient> builder)
    {
        builder.Property(i => i.Name).HasMaxLength(200);
        builder.Property(i => i.Quantity).HasMaxLength(100);
    }
}
