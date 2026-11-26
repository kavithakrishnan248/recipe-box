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

// Only origins listed in config may call the API from a browser. With none configured, cross-origin calls are blocked.
const string WebAppCorsPolicy = "WebApp";
var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];
builder.Services.AddCors(options => options.AddPolicy(WebAppCorsPolicy, policy => policy
    .WithOrigins(allowedOrigins)
    .AllowAnyHeader()
    .AllowAnyMethod()
    .WithExposedHeaders("Location")));

var app = builder.Build();

app.UseExceptionHandler();
app.UseStatusCodePages();

app.UseHttpsRedirection();

// CORS must run before authentication so preflights and 401 responses still carry CORS headers.
app.UseCors(WebAppCorsPolicy);
// app.UseAuthentication(); app.UseAuthorization(); go here, after CORS.

app.MapControllers();

app.Run();
