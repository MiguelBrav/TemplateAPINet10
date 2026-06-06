using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace TestProject.Configurations;

public static class ExceptionHandlerExtensions
{
    public static void UseGlobalExceptionHandler(this WebApplication app)
    {
        app.UseExceptionHandler(errorApp =>
        {
            errorApp.Run(async context =>
            {
                var ex = context.Features.Get<IExceptionHandlerFeature>()?.Error;

                var (status, title) = ex switch
                {
                    ArgumentException or BadHttpRequestException => (StatusCodes.Status400BadRequest, "Bad request"),
                    KeyNotFoundException => (StatusCodes.Status404NotFound, "Resource not found"),
                    TimeoutException or TaskCanceledException or OperationCanceledException => (StatusCodes.Status504GatewayTimeout, "Gateway timeout"),
                    _ => (StatusCodes.Status500InternalServerError, "Internal server error")
                };

                var problem = new ProblemDetails
                {
                    Status = status,
                    Title = title,
                    Detail = ex?.Message,
                    Instance = context.Request.Path
                };

                context.Response.ContentType = "application/problem+json";
                context.Response.StatusCode = status;
                await context.Response.WriteAsJsonAsync(problem);
            });
        });
    }
}
