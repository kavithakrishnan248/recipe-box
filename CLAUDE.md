# Recipe Box
- /api: ASP.NET Core Web API on .NET 10 (controllers, [ApiController]), EF Core with SQL Server. /web: Angular, TypeScript strict (added later).
- Thin controllers, logic in services behind interfaces, DTOs as records, ProblemDetails for errors.
- Async all the way, pass CancellationToken. Data annotations for validation.
- Photos are not part of v1. They will be added later behind an IPhotoStorage interface.
- Never hard-code secrets. The connection string comes from user-secrets (ConnectionStrings:Default).
- Explain each change before making it.