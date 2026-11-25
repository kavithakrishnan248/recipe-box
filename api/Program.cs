using Microsoft.EntityFrameworkCore;
using RecipeBox.Api.Data;
using RecipeBox.Api.Errors;
using RecipeBox.Api.Services;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Default");

// Migrations can be created without a database, so design time does not need it.
if (string.IsNullOrWhiteSpace(connectionString) && !EF.IsDesignTime)
{
    throw new InvalidOperationException(
        "ConnectionStrings:Default is not set. Set it with: " +
        "dotnet user-secrets set \"ConnectionStrings:Default\" \"<connection string>\" --project api");
}

builder.Services.AddDbContext<RecipeBoxDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddScoped<IRecipeService, RecipeService>();

builder.Services.AddControllers();
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<ConcurrencyExceptionHandler>();

var app = builder.Build();

app.UseExceptionHandler();
app.UseStatusCodePages();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
