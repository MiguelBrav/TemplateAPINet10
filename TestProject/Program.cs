using TestProject.Domain.Interfaces;
using TestProject.Endpoints;
using TestProject.Infrastructure;
using TestProject.UseCases;
using UseCaseCore.UseCases;
using TestProject.Configurations;

var builder = WebApplication.CreateBuilder(args);

// CORS - allow all (for development/testing)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy => policy
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod());
});

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddTransient<UseCaseDispatcher>();
builder.Services.AddTransient<GetEntityByIdUseCase>();
builder.Services.AddTransient<IBaseEntityRepository,DapperBaseRepository>();
builder.Services.AddTransient<CreateEntityUseCase>();

var app = builder.Build();

// Use CORS
app.UseCors("AllowAll");

// Use global exception handler
app.UseGlobalExceptionHandler();

// Map OpenAPI endpoints (only in Development)
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "v1");
    });
}

app.UseHttpsRedirection();

// Map category group endpoints using group handler
app.MapGroup("/entities").WithTags("Entity").MapEntitiesEndpoints();

app.Run();