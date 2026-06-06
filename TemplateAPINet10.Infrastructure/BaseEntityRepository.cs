using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using TemplateAPINet10.Domain.Interfaces;
using TemplateAPINet10.Models.Entities;

namespace TemplateAPINet10.Infrastructure;

public class DapperBaseRepository : IBaseEntityRepository
{
    private readonly IConfiguration _configuration;
    private readonly string _connectionString;

    public DapperBaseRepository(IConfiguration configuration)
    {
        _configuration = configuration;
        _connectionString = _configuration.GetConnectionString("MySql") ?? string.Empty;
    }

    // Template for Create using Dapper + MariaDB (example)
    public Task<int> Create(BaseEntity baseE)
    {
        throw new NotImplementedException("Implement with Dapper/MySQL: open connection and execute INSERT. Example SQL: INSERT INTO base_entities (Name) VALUES (@Name);");
    }

    // Template for GetById
    public Task<BaseEntity?> GetById(string id)
    {
        throw new NotImplementedException("Implement with Dapper/MySQL: execute SELECT by id and map result. Example SQL: SELECT Id, Name FROM base_entities WHERE Id = @Id LIMIT 1");
    }
}
