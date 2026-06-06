# TemplateAPINet10

A clean, minimal .NET 10 Web API template using **UseCaseCore** library with a **Dapper/MariaDB** infrastructure skeleton and **Minimal APIs**.

## Quick Start

```powershell
# Install template globally
dotnet new install "C:\path\to\TemplateAPINet10" --force

# Create new project from template
dotnet new apiclean10 -n MyProject
cd MyProject

# Run
dotnet run
```

The API will be available at `https://localhost:5001`

---

## Overview

This template demonstrates a clean architecture approach for building REST APIs in .NET 10 using:

- **UseCaseCore Library**: Framework-agnostic library for implementing Use Cases, Result wrappers, and a generic Dispatcher.
- **Minimal APIs**: ASP.NET Core Minimal APIs for lightweight, fast endpoint definitions.
- **Dapper Template**: Ready-to-implement repository pattern with Dapper + MySqlConnector (not implemented in template state).
- **Global Exception Handler**: Centralized error handling that returns `application/problem+json` responses.

## Project Structure

```
TemplateAPINet10/
├── Configurations/
│   └── ExceptionHandlerExtensions.cs      # Global exception handler middleware
├── Domain/
│   └── Interfaces/
│       └── IBaseEntityRepository.cs       # Base repository interface
├── Endpoints/
│   └── EntitiesEndpoints.cs               # Minimal API route group handlers
├── Infrastructure/
│   └── BaseEntityRepository.cs            # Dapper template (not implemented)
├── Models/
│   ├── DTOs/
│   │   └── BaseRecord.cs                  # Request/response DTO
│   ├── Entities/
│   │   └── BaseEntity.cs                  # Domain entity
│   └── Responses/
│       └── BaseResponse.cs                # API response DTO
├── UseCases/
│   ├── CreateEntityUseCase.cs             # Create use case (dispatches to repo)
│   └── GetEntityByIdUseCase.cs            # Get by id use case (dispatches to repo)
├── UseCaseCore/
│   ├── ResultCase.cs                      # Result wrapper with HTTP status codes
│   ├── ResultCaseExtensions.cs            # Mapping extensions to IResult
│   ├── UseCaseBase.cs                     # Abstract base class for use cases
│   └── UseCaseDispatcher.cs               # Generic dispatcher (overridable)
├── appsettings.json                       # Configuration (MySql connection string placeholder)
└── Program.cs                             # Dependency injection & middleware setup
```

## Key Features

### 1. **UseCaseCore Integration**
- `UseCaseBase<TRequest, TResponse>`: Abstract class for implementing use cases with consistent execution pattern.
- `UseCaseDispatcher`: Generic dispatcher that executes use cases; overridable for cross-cutting concerns (logging, caching, retries, domain events).
- `ResultCase<T>`: Standardized result wrapper supporting:
  - HTTP status codes (200 OK, 201 Created, 204 NoContent, 400 BadRequest, 404 NotFound, 500 ServerError)
  - Optional error messages and created resource locations
  - Extension method `ToIResult()` to map results to `IResult` for Minimal APIs.

### 2. **Minimal APIs with Group Handlers**
- Endpoints defined as static handler methods in `Endpoints/EntitiesEndpoints.cs`.
- Route group `/entities` with:
  - `GET /entities/{id}` — Retrieve entity by id (requires authorization)
  - `POST /entities` — Create entity (requires authorization)
- Handlers inject use cases and dispatcher for clean, declarative endpoint definitions.

### 3. **Dapper Repository Template**
- `DapperBaseRepository` implements `IBaseEntityRepository` but throws `NotImplementedException`.
- Example SQL and usage comments included for easy implementation with Dapper + MySqlConnector.
- Connection string read from configuration: `IConfiguration.GetConnectionString("MySql")`.

### 4. **Global Exception Handler**
- Centralized error handling via `ExceptionHandlerExtensions.UseGlobalExceptionHandler()`.
- Maps common exceptions to HTTP status codes:
  - `ArgumentException` / `BadHttpRequestException` → 400 Bad Request
  - `KeyNotFoundException` → 404 Not Found
  - `TimeoutException` / `TaskCanceledException` / `OperationCanceledException` → 504 Gateway Timeout
  - All others → 500 Internal Server Error
- Responses use `application/problem+json` format (RFC 7807 ProblemDetails).

## Getting Started

### Prerequisites
- .NET 10 SDK
- Visual Studio 2022+ (or any .NET IDE)

### Installation

#### Option 1: Install Template Globally (Recommended)

1. **Install the template** from the source directory:
   ```powershell
   dotnet new install "C:\path\to\TemplateAPINet10" --force
   ```
   Replace with your actual path to the TemplateAPINet10 repository.

2. **Verify installation**:
   ```powershell
   dotnet new list | findstr apiclean10
   ```
   You should see: `Clean API .NET 10 Template [C#] apiclean10`

3. **Create a new project** from the template:
   ```powershell
   dotnet new apiclean10 -n MyNewProject
   cd MyNewProject
   dotnet build
   dotnet run
   ```

#### Option 2: Clone and Customize Directly
1. **Clone/download the repository**.
2. **Rename all projects and folders** from `TemplateAPINet10` to your project name.
3. Open in Visual Studio or your IDE.

### Build & Run

1. **Restore dependencies**:
   ```powershell
   dotnet restore
   ```

2. **Build**:
   ```powershell
   dotnet build
   ```

3. **Run**:
   ```powershell
   dotnet run
   ```

   The API will be available at `https://localhost:5001` (by default).

## Using the Endpoints

### GET /entities/{id}
Retrieve an entity by ID.

**Request**:
```bash
curl -X GET https://localhost:5001/entities/1
```

**Response** (200 OK):
```json
{
  "id": 1,
  "name": "Example Entity"
}
```

**Response** (404 Not Found):
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.4",
  "title": "Resource not found",
  "status": 404,
  "detail": "Entity not found",
  "instance": "/entities/999"
}
```

---

### POST /entities
Create a new entity.

**Request**:
```bash
curl -X POST https://localhost:5001/entities \
  -H "Content-Type: application/json" \
  -d '{"name": "New Entity"}'
```

**Response** (201 Created):
```json
{
  "id": 1,
  "name": "New Entity"
}
```

**Response** (501 Not Implemented):
```json
{
  "status": 501,
  "title": "Internal server error",
  "detail": "Implement with Dapper/MySQL: open connection and execute INSERT...",
  "instance": "/entities"
}
```

---

## Implementing the Repository

To implement `DapperBaseRepository` with real database operations:

### 1. Install NuGet Packages
```powershell
dotnet add package Dapper
dotnet add package MySqlConnector
```

### 2. Configure Connection String
Add to `appsettings.json` or `appsettings.Development.json`:
```json
{
  "ConnectionStrings": {
	"MySql": "Server=localhost;Database=your_db;User=your_user;Password=your_password;"
  }
}
```

Or use **User Secrets** for development:
```powershell
dotnet user-secrets set "ConnectionStrings:MySql" "Server=localhost;Database=your_db;User=your_user;Password=your_password;"
```

### 3. Implement `Create` Method
Replace the `throw` in `DapperBaseRepository.Create()` with:
```csharp
public async Task<int> Create(BaseEntity baseE)
{
	using var conn = new MySqlConnector.MySqlConnection(_connectionString);
	await conn.OpenAsync();

	var sql = "INSERT INTO base_entities (Name) VALUES (@Name); SELECT LAST_INSERT_ID();";
	var id = await conn.ExecuteScalarAsync<int>(sql, new { Name = baseE.Name });

	return id;
}
```

### 4. Implement `GetById` Method
Replace the `throw` in `DapperBaseRepository.GetById()` with:
```csharp
public async Task<BaseEntity?> GetById(string id)
{
	if (!int.TryParse(id, out var intId))
		return null;

	using var conn = new MySqlConnector.MySqlConnection(_connectionString);
	await conn.OpenAsync();

	var sql = "SELECT Id, Name FROM base_entities WHERE Id = @Id LIMIT 1";
	var item = await conn.QuerySingleOrDefaultAsync<BaseEntity>(sql, new { Id = intId });

	return item;
}
```

### 5. Create Database Table
```sql
CREATE TABLE base_entities (
	Id INT AUTO_INCREMENT PRIMARY KEY,
	Name VARCHAR(255) NOT NULL
);
```

## Adding More Endpoints

To add new endpoints, follow this pattern:

1. **Create a Use Case** (e.g., `UseCases/UpdateEntityUseCase.cs`):
   ```csharp
   public class UpdateEntityUseCase : UseCaseBase<(string Id, BaseRecord Data), IResult>
   {
	   private readonly IBaseEntityRepository _repository;

	   public UpdateEntityUseCase(IBaseEntityRepository repository)
	   {
		   _repository = repository;
	   }

	   public override async Task<IResult> Execute((string Id, BaseRecord Data) request)
	   {
		   // Implement update logic
	   }
   }
   ```

2. **Register the Use Case** in `Program.cs`:
   ```csharp
   builder.Services.AddTransient<UpdateEntityUseCase>();
   ```

3. **Add the endpoint handler** in `Endpoints/EntitiesEndpoints.cs`:
   ```csharp
   group.MapPut("/{id}", Update);

   static async Task<IResult> Update(string id, BaseRecord record, UpdateEntityUseCase useCase, UseCaseDispatcher dispatcher, HttpContext httpContext)
   {
	   return await dispatcher.Dispatch(useCase, (id, record));
   }
   ```

## Customizing the Dispatcher

To add cross-cutting concerns (logging, retries, caching, metrics), create a custom dispatcher:

```csharp
public class CustomDispatcher : UseCaseDispatcher
{
	private readonly ILogger<CustomDispatcher> _logger;

	public CustomDispatcher(ILogger<CustomDispatcher> logger)
	{
		_logger = logger;
	}

	public override async Task<TResponse> Dispatch<TRequest, TResponse>(UseCaseBase<TRequest, TResponse> useCase, TRequest request)
	{
		_logger.LogInformation("Executing {UseCaseName}", useCase.GetType().Name);

		try
		{
			return await useCase.Execute(request);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error in {UseCaseName}", useCase.GetType().Name);
			throw;
		}
	}
}
```

Register it in `Program.cs`:
```csharp
builder.Services.AddTransient<UseCaseDispatcher, CustomDispatcher>();
```

## Configuration

### appsettings.json
- **Logging**: Log level configuration
- **AllowedHosts**: Allowed hostnames
- **ConnectionStrings** (placeholder): MySql connection string for Dapper

### CORS
CORS is configured in `Program.cs` to allow all origins for development. **Restrict this in production** by modifying the policy:
```csharp
options.AddPolicy("AllowAll", policy => policy
	.WithOrigins("https://yourdomain.com")
	.AllowAnyMethod()
	.AllowAnyHeader());
```

## Notes

- This is a **template/skeleton** — repository methods are not implemented; they throw `NotImplementedException` until you add Dapper.
- All code is in **English** following .NET conventions.
- The template uses **Minimal APIs** for lightweight, declarative endpoint definitions.
- The **global exception handler** automatically maps exceptions to standardized HTTP responses.
- The **UseCaseCore dispatcher** is a central point for adding cross-cutting logic without modifying use cases.

## License

This template is open for modification and use as a base for your projects.

---

**Happy coding!** 🚀
