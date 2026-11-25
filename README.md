# recipe-box
Store and browse collection of recipes

## How to run

You need the .NET 10 SDK and a SQL Server instance. From the repo root:

```powershell
dotnet tool restore
dotnet user-secrets set "ConnectionStrings:Default" "<your connection string>" --project api
dotnet ef database update --project api
dotnet run --project api --launch-profile http
```

The API listens on http://localhost:5080. To try it, open `api/recipes.http` in VS Code with the REST Client extension and click "Send Request".
