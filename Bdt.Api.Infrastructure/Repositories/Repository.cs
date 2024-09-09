using AutoMapper;
using Bdt.Api.Domain.Entities;
using Bdt.Api.Domain.Enums;
using Bdt.Api.Infrastructure.Context;
using Bdt.Api.Infrastructure.Exceptions.Database;
using Bdt.Api.Infrastructure.Repositories.Interfaces;
using Bdt.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using System.Reflection;

namespace Bdt.Api.Infrastructure.Repositories;

public class ReadRepository<TId, TEntity> : IReadRepository<TId, TEntity>
    where TEntity : class, IEntity<TId>
{
    internal readonly BdtDbContext _context;
    internal readonly IMapper _mapper;
    internal readonly ILogger _logger;

    public ReadRepository(BdtDbContext context, IMapper mapper, ILogger<ReadRepository<TId, TEntity>> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public IEnumerable<TEntity> GetAll()
    {
        return ExecuteDbOperation(context => context.Set<TEntity>().ToList(), "GetAll", DbOperationEnum.Get);
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await ExecuteDbOperationAsync(context => context.Set<TEntity>().ToListAsync(), "GetAllAsync", DbOperationEnum.Get);
    }

    public async Task<IEnumerable<TEntity>> GetAllWithIncludesAsync()
    {
        return await ExecuteDbOperationAsync(async _context =>
        {
            var entityType = _context.Model.FindEntityType(typeof(TEntity))
                ?? throw new BdtNotFoundException($"Entity of type {typeof(TEntity).Name} not found.");

            var navigationProperties = entityType.GetNavigations();
            var query = _context.Set<TEntity>().AsQueryable();

            foreach (var navigationProperty in navigationProperties)
                query = query.Include(navigationProperty.Name);

            var entities = await query.ToListAsync();

            return entities;
        }, "GetAllWithIncludesAsync", DbOperationEnum.Get);
        
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, object>>[]? includes = null, Expression<Func<TEntity, bool>>[]? conditions = null,
        Func<TEntity, object>? orderBy = null, OrderByDirectionEnum? orderDirection = null)
    {
        return await ExecuteDbOperationAsync(async _context =>
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

            if (includes != null && includes.Any())
                foreach (var include in includes)
                    query = query.Include(include);

            if (conditions != null && conditions.Any())
                foreach (var condition in conditions)
                    query = query.Where(condition);

            if (orderBy != null && orderDirection != null)
            {
                if (orderDirection == OrderByDirectionEnum.Ascending)
                    query = query.OrderBy(orderBy).AsQueryable();
                else
                    query = query.OrderByDescending(orderBy).AsQueryable();
            }

            var entities = await query.ToListAsync();

            return entities;
        }, "GetAllAsync", DbOperationEnum.Get);
    }

    public async Task<IEnumerable<TEntity>?> GetAllByExpression(Expression<Func<TEntity, bool>> toCheck)
    {
        return await ExecuteDbOperationAsync(async _context =>
        {
            var entities = await _context.Set<TEntity>().Where(toCheck).ToListAsync();

            return entities;
        }, "GetAllByExpression", DbOperationEnum.Get);
    }

    public async Task<IEnumerable<TEntity>?> GetAllByExpressionWithIncludes(Expression<Func<TEntity, bool>> toCheck)
    {
        return await ExecuteDbOperationAsync(async _context =>
        {
            var entityType = _context.Model.FindEntityType(typeof(TEntity))
                ?? throw new BdtNotFoundException($"Entity of type {typeof(TEntity).Name} not found.");

            var navigationProperties = entityType.GetNavigations();
            var query = _context.Set<TEntity>().AsQueryable();

            foreach (var navigationProperty in navigationProperties)
                query = query.Include(navigationProperty.Name);

            query = query.Where(toCheck);

            var entities = await query.ToListAsync();

            return entities;
        }, "GetAllByExpressionWithIncludes", DbOperationEnum.Get);
    }

    public IEnumerable<TEntity> Paging(int pageSize, int pageNumber, Expression<Func<TEntity, object>>[]? includes = null,
        Expression<Func<TEntity, bool>>[]? conditions = null, Func<TEntity, object>? orderBy = null, OrderByDirectionEnum? orderDirection = null)
    {
        return ExecuteDbOperation(_context =>
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

            if (includes != null)
                foreach (var include in includes)
                    query = query.Include(include);

            if (conditions != null)
                foreach (var condition in conditions)
                    query = query.Where(condition);

            if (orderBy != null && orderDirection != null)
            {
                if (orderDirection == OrderByDirectionEnum.Ascending)
                    query = query.OrderBy(orderBy).AsQueryable();
                else
                    query = query.OrderByDescending(orderBy).AsQueryable();
            }

            var entities = query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            return entities;
        }, "Paging", DbOperationEnum.Get);
    }

    public IEnumerable<TEntity> PagingWithIncludes(int pageSize, int pageNumber, Expression<Func<TEntity, object>>[]? includes = null,
        Expression<Func<TEntity, bool>>[]? conditions = null, Func<TEntity, object>? orderBy = null, OrderByDirectionEnum? orderDirection = null)
    {
        return ExecuteDbOperation(_context =>
        {
            var entityType = _context.Model.FindEntityType(typeof(TEntity))
                ?? throw new BdtNotFoundException($"Entity of type {typeof(TEntity).Name} not found.");

            var navigationProperties = entityType.GetNavigations();

            IQueryable<TEntity> query = _context.Set<TEntity>();

            foreach (var navigationProperty in navigationProperties)
                query = query.Include(navigationProperty.Name);

            if (includes != null)
                foreach (var include in includes)
                    query = query.Include(include);

            if (conditions != null)
                foreach (var condition in conditions)
                    query = query.Where(condition);

            if (orderBy != null && orderDirection != null)
            {
                if (orderDirection == OrderByDirectionEnum.Ascending)
                    query = query.OrderBy(orderBy).AsQueryable();
                else
                    query = query.OrderByDescending(orderBy).AsQueryable();
            }

            var entities = query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            return entities;
        }, "PagingWithIncludes", DbOperationEnum.Get);
    }

    public async Task<int> CountByConditionAsync(Expression<Func<TEntity, bool>>[] conditions)
    {
        return await ExecuteDbOperationAsync(async _context =>
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

            if (conditions.Any())
                foreach (var condition in conditions)
                    query = query.Where(condition);

            return await query.CountAsync();
        }, "CountByConditionAsync", DbOperationEnum.Get);
    }

    public int Count()
    {
        return ExecuteDbOperation(_context =>
        {
            return _context.Set<TEntity>().Count();
        }, "Count", DbOperationEnum.Get);
    }

    public async Task<int> CountAsync()
    {
        return await ExecuteDbOperationAsync(async _context =>
        {
            return await _context.Set<TEntity>().CountAsync();
        }, "CountAsync", DbOperationEnum.Get);
    }

    public bool IdExists(TId id)
    {
        return ExecuteDbOperation(_context =>
        {
            return _context.Set<TEntity>().Any(CreateEqualityExpressionForId(id));
        }, "IdExists", DbOperationEnum.Get);
    }

    public virtual async Task<bool> IdExistsAsync(TId id)
    {
        return await ExecuteDbOperationAsync(async _context =>
        {
            return await _context.Set<TEntity>().AnyAsync(CreateEqualityExpressionForId(id));
        }, "IdExistsAsync", DbOperationEnum.Get);
    }

    public virtual TEntity? GetById(TId id)
    {
        return ExecuteDbOperation(_context =>
        {
            var entity = _context.Set<TEntity>().Find(id);

            return entity;
        }, "GetById", DbOperationEnum.Get);
    }

    public virtual IEnumerable<TEntity?> GetManyByIds(IEnumerable<TId> ids)
    {
        return ExecuteDbOperation(_context =>
        {
            var entities = _context.Set<TEntity>().Where(x => ids.Contains(x.Id));

            return entities;
        }, "GetManyByIds", DbOperationEnum.Get);
    }

    public virtual async Task<TEntity?> GetByIdAsync(TId id)
    {
        return await ExecuteDbOperationAsync(async _context =>
        {
            var entity = await _context.Set<TEntity>().FindAsync(id);

            return entity;
        }, "GetByIdAsync", DbOperationEnum.Get);
    }

    public virtual async Task<TEntity?> GetByIdWithIncludesAsync(TId id)
    {
        return await ExecuteDbOperationAsync(async _context =>
        {
            var entityType = _context.Model.FindEntityType(typeof(TEntity))
                ?? throw new BdtNotFoundException($"Entity of type {typeof(TEntity).Name} not found.");

            var navigationProperties = entityType.GetNavigations();
            var query = _context.Set<TEntity>().AsQueryable();

            foreach (var navigationProperty in navigationProperties)
                query = query.Include(navigationProperty.Name);

            var entity = await query.FirstOrDefaultAsync(CreateEqualityExpressionForId(id));

            return entity;
        }, "GetByIdWithIncludesAsync", DbOperationEnum.Get);
    }

    public virtual async Task<IEnumerable<TEntity?>> GetManyByIdsAsync(IEnumerable<TId> ids)
    {
        return await ExecuteDbOperationAsync(async _context =>
        {
            var entity = await _context.Set<TEntity>().Where(x => ids.Contains(x.Id)).ToListAsync();

            return entity;
        }, "GetManyByIdsAsync", DbOperationEnum.Get);
    }

    public virtual async Task<TEntity?> GetByIdAsync(TId id, Expression<Func<TEntity, object>>[] includes)
    {
        return await ExecuteDbOperationAsync(async _context =>
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

            if (includes.Any())
                foreach (var include in includes)
                    query = query.Include(include);

            query = query.AsQueryable();

            var entity = await query.FirstOrDefaultAsync(CreateEqualityExpressionForId(id));

            return entity;
        }, "GetByIdAsync", DbOperationEnum.Get);
    }

    public virtual async Task<bool> SomethingExists(Expression<Func<TEntity, bool>> toCheck)
    {
        return await ExecuteDbOperationAsync(async _context =>
        {
            return await _context.Set<TEntity>().AnyAsync(toCheck);

        }, "SomethingExists", DbOperationEnum.Get);
    }

    public virtual async Task<TEntity?> GetEntityByExpression(Expression<Func<TEntity, bool>> toCheck)
    {
        return await ExecuteDbOperationAsync(async _context =>
        {
            var entity = await _context.Set<TEntity>().FirstOrDefaultAsync(toCheck);

            return entity;
        }, "GetEntityByExpression", DbOperationEnum.Get);
    }

    private Expression<Func<TEntity, bool>> CreateEqualityExpressionForId(TId id)
    {
        var lambdaParam = Expression.Parameter(typeof(TEntity));

        var leftExpression = Expression.PropertyOrField(lambdaParam, "Id");

        var idValue = Convert.ChangeType(id, typeof(TId))
            ?? throw new BdtComparisonException($"Change of {id} to type {typeof(TId).Name} returned null.");

        Expression<Func<object>> closure = () => idValue;
        var rightExpression = Expression.Convert(closure.Body, leftExpression.Type);

        var lambdaBody = Expression.Equal(leftExpression, rightExpression);

        return Expression.Lambda<Func<TEntity, bool>>(lambdaBody, lambdaParam);
    }

    protected async Task<TResult> ExecuteDbOperationAsync<TResult>(Func<BdtDbContext, Task<TResult>> operation, string operationDescription, DbOperationEnum operationEnum)
    {
        try
        {
            return await operation(_context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to execute operation {operationDescription} on {typeof(TEntity).Name}.");
            throw new DbOperationFailedException(operationEnum, $"Failed to execute operation {operationDescription} on {typeof(TEntity).Name}.", ex);
        }
    }

    public TResult ExecuteDbOperation<TResult>(Func<BdtDbContext, TResult> operation, string operationDescription, DbOperationEnum operationEnum)
    {
        try
        {
            return operation(_context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to execute operation {operationDescription} on {typeof(TEntity).Name}.");
            throw new DbOperationFailedException(operationEnum, $"Failed to execute operation {operationDescription} on {typeof(TEntity).Name}.", ex);
        }
    }
}

public class UpdateRepository<TId, TEntity> : ReadRepository<TId, TEntity>, IUpdateRepository<TId, TEntity>
    where TEntity : class, IEntity<TId>
{
    public UpdateRepository(BdtDbContext context, IMapper mapper, ILogger<UpdateRepository<TId, TEntity>> logger)
        : base(context, mapper, logger)
    { }

    public bool Update(TEntity entity)
    {
        return ExecuteDbOperation(_context =>
        {
            var table = _context.Set<TEntity>();
            using var transaction = _context.Database.BeginTransaction();

            var tracking = _context.ChangeTracker.Entries<TEntity>().Any(x => x.Entity.Id.Equals(entity.Id));
            if (!tracking)
            {
                table.Update(entity);
            }

            bool updated = _context.SaveChanges() > 0;

            transaction.Commit();

            table.Entry(entity).State = EntityState.Detached;
            return updated;
        }, "Update", DbOperationEnum.Update);
    }

    public bool UpdateMany(IEnumerable<TEntity> entities)
    {
        return ExecuteDbOperation(_context =>
        {
            var table = _context.Set<TEntity>();
            using var transaction = _context.Database.BeginTransaction();
            foreach (var entity in entities)
            {
                var tracking = _context.ChangeTracker.Entries<TEntity>().Any(x => x.Entity.Id.Equals(entity.Id));
                if (tracking)
                {
                    return false;
                }
            }

            table.UpdateRange(entities);

            bool updated = _context.SaveChanges() > 0;

            transaction.Commit();
            foreach (var entity in entities)
            {
                table.Entry(entity).State = EntityState.Detached;
            }

            return updated;
        }, "UpdateMany", DbOperationEnum.Update);
    }

    public bool Save()
    {
        return _context.SaveChanges() > 0;
    }

    public async Task<bool> SaveAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}

public class CreateRepository<TId, TEntity> : UpdateRepository<TId, TEntity>, ICreateRepository<TId, TEntity>
    where TEntity : class, IEntity<TId>
{
    public CreateRepository(BdtDbContext context, IMapper mapper, ILogger<CreateRepository<TId, TEntity>> logger)
        : base(context, mapper, logger)
    { }

    public TEntity Insert(TEntity entity)
    {
        return ExecuteDbOperation(_context =>
        {
            _context.Set<TEntity>().Add(entity);

            return entity;
        }, "Insert", DbOperationEnum.Insert);
    }

    public async Task<TEntity> InsertAsync(TEntity entity)
    {
        return await ExecuteDbOperationAsync(async _context =>
        {
            await _context.Set<TEntity>().AddAsync(entity);

            if (!await SaveAsync())
                throw new Exception("Could not be added to database.");

            var dto = await GetByIdWithIncludesAsync(entity.Id)
                ?? throw new Exception("Entity was not added to database.");

            return dto;
        }, "InsertAsync", DbOperationEnum.Insert);
    }

    public IEnumerable<TEntity> InsertMany(IEnumerable<TEntity> entities)
    {
        return ExecuteDbOperation(_context =>
        {
            _context.Set<TEntity>().AddRange(entities);

            if (!(_context.SaveChanges() > 0))
                throw new Exception("Failed to create new entities.");

            return entities;
        }, "InsertMany", DbOperationEnum.Insert);
    }

    public async Task<IEnumerable<TEntity>> InsertManyAsync(IEnumerable<TEntity> entities)
    {
        return await ExecuteDbOperationAsync(async _context =>
        {
            await _context.Set<TEntity>().AddRangeAsync(entities);

            if (!await SaveAsync())
                throw new Exception("Failed to create new entities.");

            return entities;
        }, "InsertManyAsync", DbOperationEnum.Insert);
    }

    public bool InsertOrUpdateMany(IEnumerable<TEntity> entities)
    {
        return ExecuteDbOperation(_context =>
        {
            var table = _context.Set<TEntity>();
            using var transaction = _context.Database.BeginTransaction();

            PropertyInfo propInfo = entities.First().GetType().GetProperty("Id");
            var type = propInfo.PropertyType;
            bool isLong = type.FullName == "System.Int64";

            table.UpdateRange(entities);

            bool updated = _context.SaveChanges() > 0;

            transaction.Commit();

            return updated;

        }, "InsertOrUpdateMany", DbOperationEnum.Insert);
    }
}

public class DeleteRepository<TId, TEntity> : CreateRepository<TId, TEntity>, IDeleteRepository<TId, TEntity>
    where TEntity : class, IEntity<TId>
{
    public DeleteRepository(BdtDbContext context, IMapper mapper, ILogger<DeleteRepository<TId, TEntity>> logger)
        : base(context, mapper, logger)
    { }

    public bool Delete(TEntity entity)
    {
        return ExecuteDbOperation(_context =>
        {
            _context.Set<TEntity>().Remove(entity);
            return Save();
        }, "Delete", DbOperationEnum.Delete);
    }

    public bool DeleteMany(IEnumerable<TEntity> entities)
    {
        return ExecuteDbOperation(_context =>
        {
            _context.Set<TEntity>().RemoveRange(entities);
            return Save();
        }, "DeleteMany", DbOperationEnum.Delete);
    }

    public async Task<bool> DeleteAsync(TEntity entity)
    {
        return await ExecuteDbOperationAsync(async _context =>
        {
            await Task.Run(() =>
            {
                _context.Set<TEntity>().Remove(entity);
            });

            return await SaveAsync();
        }, "DeleteAsync", DbOperationEnum.Delete);
    }

    public async Task<bool> DeleteByIdAsync(TId id)
    {
        return await ExecuteDbOperationAsync(async _context =>
        {
            var table = _context.Set<TEntity>();
            TEntity existing = await table.FindAsync(id) ??
            throw new Exception($"ID {id} not found");

            table.Remove(existing);

            return await SaveAsync();
        }, "DeleteByIdAsync", DbOperationEnum.Delete);
    }

    public async Task<bool> DeleteManyAsync(IEnumerable<TEntity> entities)
    {
        return await ExecuteDbOperationAsync(async _context =>
        {
            await Task.Run(() => { _context.Set<TEntity>().RemoveRange(entities); });
            return await SaveAsync();
        }, "DeleteManyAsync", DbOperationEnum.Delete);
    }
}