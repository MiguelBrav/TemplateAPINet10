using TestProject.Domain.Interfaces;
using TestProject.Models.DTOs;
using TestProject.Models.Entities;
using UseCaseCore.UseCases;

namespace TestProject.UseCases;

public class CreateEntityUseCase : UseCaseBase<BaseRecord, IResult>
{
    private readonly IBaseEntityRepository _repository;

    public CreateEntityUseCase(IBaseEntityRepository repository)
    {
        _repository = repository;
    }

    public override async Task<IResult> Execute(BaseRecord request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            return TypedResults.BadRequest("Name is required");

        try
        {
            var entity = new BaseEntity { Name = request.Name };
            var id = await _repository.Create(entity);
            return TypedResults.Created($"/entities/{id}", new { Id = id, entity.Name });
        }
        catch (Exception ex)
        {
            return TypedResults.Problem(ex.Message);
        }
    }
}
