using TemplateAPINet10.Models.Entities;

namespace TemplateAPINet10.Domain.Interfaces;

public interface IBaseEntityRepository
{
    Task<int> Create(BaseEntity baseE);
    Task<BaseEntity?> GetById(string id);
}
