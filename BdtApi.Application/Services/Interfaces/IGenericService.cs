using BdtApi.Domain.Entities;
using BdtShared.Dtos;
using BdtShared.Enums;
using System.Linq.Expressions;

namespace BdtApi.Application.Services.Interfaces;

public interface IGenericService<TId, TEntity, TDto>
    where TEntity : class, IEntity<TId>
    where TDto : class, IBaseDto<TId>
{
    IEnumerable<TDto> GetAll();
    Task<IEnumerable<TDto>> GetAllAsync();
    Task<IEnumerable<TDto>> GetAllWithIncludesAsync();
    Task<IEnumerable<TDto>> GetAllAsync(Expression<Func<TEntity, object>>[]? includes = null, Expression<Func<TEntity, bool>>[]? conditions = null,
        Func<TEntity, object>? orderBy = null, OrderByDirectionEnum? orderDirection = null);
    Task<IEnumerable<TDto>?> GetAllByExpression(Expression<Func<TEntity, bool>> toCheck);
    Task<IEnumerable<TDto>?> GetAllByExpressionWithIncludes(Expression<Func<TEntity, bool>> toCheck);
    IEnumerable<TDto> Paging(int pageSize, int pageNumber, Expression<Func<TEntity, object>>[]? includes = null,
        Expression<Func<TEntity, bool>>[]? conditions = null, Func<TEntity, object>? orderBy = null, OrderByDirectionEnum? orderDirection = null);
    IEnumerable<TDto> PagingWithIncludes(int pageSize, int pageNumber, Expression<Func<TEntity, object>>[]? includes = null,
        Expression<Func<TEntity, bool>>[]? conditions = null, Func<TEntity, object>? orderBy = null, OrderByDirectionEnum? orderDirection = null);
    Task<int> CountByConditionAsync(Expression<Func<TEntity, bool>>[] conditions);
    int Count();
    Task<int> CountAsync();
    bool IdExists(TId id);
    Task<bool> IdExistsAsync(TId id);
    TDto? GetById(TId id);
    IEnumerable<TDto?> GetManyByIds(IEnumerable<TId> ids);
    Task<TDto?> GetByIdAsync(TId id);
    Task<TDto?> GetByIdWithIncludesAsync(TId id);
    Task<IEnumerable<TDto?>> GetManyByIdsAsync(IEnumerable<TId> ids);
    Task<TDto?> GetByIdAsync(TId id, Expression<Func<TEntity, object>>[] includes);
    Task<bool> SomethingExists(Expression<Func<TEntity, bool>> toCheck);
    Task<TDto?> GetEntityByExpression(Expression<Func<TEntity, bool>> toCheck);
}

public interface IGenericService<TId, TEntity, TDto, TUpdateDto> : IGenericService<TId, TEntity, TDto>
    where TEntity : class, IEntity<TId>
    where TDto : class, IBaseDto<TId>
    where TUpdateDto : class, IBaseDto<TId>
{
    bool Update(TUpdateDto obj);
    bool UpdateMany(IEnumerable<TUpdateDto> objs);
    bool Save();
    Task<bool> SaveAsync();
}

public interface IGenericService<TId, TEntity, TDto, TUpdateDto, TCreateDto> : IGenericService<TId, TEntity, TDto, TUpdateDto>
    where TEntity : class, IEntity<TId>
    where TDto : class, IBaseDto<TId>
    where TUpdateDto : class, IBaseDto<TId>
{
    TDto Insert(TCreateDto obj);
    Task<TDto> InsertAsync(TCreateDto obj);
    IEnumerable<TDto> InsertMany(IEnumerable<TCreateDto> objs);
    Task<IEnumerable<TDto>> InsertManyAsync(IEnumerable<TCreateDto> objs);
    bool InsertOrUpdateMany(IEnumerable<TCreateDto> objs);
}

public interface IGenericService<TId, TEntity, TDto, TUpdateDto, TCreateDto, TDeleteDto> : IGenericService<TId, TEntity, TDto, TUpdateDto, TCreateDto>
    where TEntity : class, IEntity<TId>
    where TDto : class, IBaseDto<TId>
    where TUpdateDto : class, IBaseDto<TId>
    where TDeleteDto : class, IBaseDto<TId>
{
    bool Delete(TDeleteDto obj);
    bool DeleteMany(IEnumerable<TDeleteDto> objs);
    Task<bool> DeleteAsync(TDeleteDto obj);
    Task<bool> DeleteByIdAsync(TId id);
}