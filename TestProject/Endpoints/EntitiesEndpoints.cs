using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using TestProject.Models.DTOs;
using TestProject.UseCases;
using UseCaseCore.UseCases;

namespace TestProject.Endpoints;

public static class EntitiesEndpoints
{
    public static RouteGroupBuilder MapEntitiesEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/{id}", GetById);

        group.MapPost("/", Create);

        return group;
    }

    static async Task<IResult> Create(BaseRecord record, CreateEntityUseCase useCase, UseCaseDispatcher dispatcher, HttpContext httpContext)
    {
        return await dispatcher.Dispatch(useCase, record);
    }

    static async Task<IResult> GetById(string id, GetEntityByIdUseCase useCase, UseCaseDispatcher dispatcher, HttpContext httpContext)
    {
        return await dispatcher.Dispatch(useCase, id);
    }
}
