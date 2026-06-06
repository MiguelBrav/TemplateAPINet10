using TestProject.Domain.Interfaces;
using TestProject.Models.Responses;
using UseCaseCore.UseCases;

namespace TestProject.UseCases;

public class GetEntityByIdUseCase : UseCaseBase<string, IResult>
{
    private readonly IBaseEntityRepository _repository;

    public GetEntityByIdUseCase(IBaseEntityRepository repository)
    {
        _repository = repository;
    }

    public override async Task<IResult> Execute(string id)
    {
        var entity = await _repository.GetById(id);
        if (entity == null)
            return TypedResults.NotFound("Entity not found");

        var dto = new BaseResponse(entity.Id, entity.Name);
        return TypedResults.Ok(dto);
    }
}
