using TemplateAPINet10.Domain.Interfaces;
using TemplateAPINet10.Models.Responses;
using UseCaseCore.UseCases;

namespace TemplateAPINet10.UseCases;

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
