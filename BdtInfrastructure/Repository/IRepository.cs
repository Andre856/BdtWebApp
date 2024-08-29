using BdtDomain.Entities;
using BdtDomain.Enums;
using BdtDomain.Dtos;
using System.Linq.Expressions;

namespace BdtDomain.Repository;

public interface IReadRepository<TId, TEntity>
    where TEntity : class, IEntity<TId>
{
    IEnumerable<TEntity> GetAll();
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<IEnumerable<TEntity>> GetAllWithIncludesAsync();
    Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, object>>[]? includes = null, Expression<Func<TEntity, bool>>[]? conditions = null,
        Func<TEntity, object>? orderBy = null, OrderByDirectionEnum? orderDirection = null);
    Task<IEnumerable<TEntity>?> GetAllByExpression(Expression<Func<TEntity, bool>> toCheck);
    Task<IEnumerable<TEntity>?> GetAllByExpressionWithIncludes(Expression<Func<TEntity, bool>> toCheck);
    IEnumerable<TEntity> Paging(int pageSize, int pageNumber, Expression<Func<TEntity, object>>[]? includes = null,
        Expression<Func<TEntity, bool>>[]? conditions = null, Func<TEntity, object>? orderBy = null, OrderByDirectionEnum? orderDirection = null);
    IEnumerable<TEntity> PagingWithIncludes(int pageSize, int pageNumber, Expression<Func<TEntity, object>>[]? includes = null,
        Expression<Func<TEntity, bool>>[]? conditions = null, Func<TEntity, object>? orderBy = null, OrderByDirectionEnum? orderDirection = null);
    Task<int> CountByConditionAsync(Expression<Func<TEntity, bool>>[] conditions);
    int Count();
    Task<int> CountAsync();
    bool IdExists(TId id);
    Task<bool> IdExistsAsync(TId id);
    TEntity? GetById(TId id);
    IEnumerable<TEntity?> GetManyByIds(IEnumerable<TId> ids);
    Task<TEntity?> GetByIdAsync(TId id);
    Task<TEntity?> GetByIdWithIncludesAsync(TId id);
    Task<IEnumerable<TEntity?>> GetManyByIdsAsync(IEnumerable<TId> ids);
    Task<TEntity?> GetByIdAsync(TId id, Expression<Func<TEntity, object>>[] includes);
    Task<bool> SomethingExists(Expression<Func<TEntity, bool>> toCheck);
    Task<TEntity?> GetEntityByExpression(Expression<Func<TEntity, bool>> toCheck);
}

public interface IUpdateRepository<TId, TEntity> : IReadRepository<TId, TEntity>
    where TEntity : class, IEntity<TId>
{
    bool Update(TEntity obj);
    bool UpdateMany(IEnumerable<TEntity> objs);
    bool Save();
    Task<bool> SaveAsync();
}

public interface ICreateRepository<TId, TEntity> : IUpdateRepository<TId, TEntity>
    where TEntity : class, IEntity<TId>
{
    TEntity Insert(TEntity obj);
    Task<TEntity> InsertAsync(TEntity obj);
    IEnumerable<TEntity> InsertMany(IEnumerable<TEntity> objs);
    Task<IEnumerable<TEntity>> InsertManyAsync(IEnumerable<TEntity> objs);
    bool InsertOrUpdateMany(IEnumerable<TEntity> objs);
}

public interface IDeleteRepository<TId, TEntity> : ICreateRepository<TId, TEntity>
    where TEntity : class, IEntity<TId>
{
    bool Delete(TEntity obj);
    bool DeleteMany(IEnumerable<TEntity> objs);
    Task<bool> DeleteAsync(TEntity obj);
    Task<bool> DeleteByIdAsync(TId id);
}