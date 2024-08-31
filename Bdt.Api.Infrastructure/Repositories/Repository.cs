using AutoMapper;
using Bdt.Api.Infrastructure.Context;
using Bdt.Api.Infrastructure.Repositories.Interfaces;
using Bdt.Api.Domain.Entities;
using Bdt.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;

namespace Bdt.Api.Infrastructure.Repositories;

public class ReadRepository<TId, TEntity> : IReadRepository<TId, TEntity>
    where TEntity : class, IEntity<TId>
{
    internal DbSet<TEntity> Table { get; set; }

    internal readonly BdtDbContext _context;
    internal readonly IMapper _mapper;

    public ReadRepository(BdtDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
        Table = _context.Set<TEntity>();
    }

    public IEnumerable<TEntity> GetAll()
    {
        var entities = Table.ToList();

        return entities;
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        var entities = await Table.ToListAsync();

        return entities;
    }

    public async Task<IEnumerable<TEntity>> GetAllWithIncludesAsync()
    {
        var entityType = _context.Model.FindEntityType(typeof(TEntity));

        var navigationProperties = entityType.GetNavigations();
        var query = Table.AsQueryable();

        foreach (var navigationProperty in navigationProperties)
            query = query.Include(navigationProperty.Name);

        var entities = await query.ToListAsync();

        return entities;
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, object>>[]? includes = null, Expression<Func<TEntity, bool>>[]? conditions = null,
        Func<TEntity, object>? orderBy = null, OrderByDirectionEnum? orderDirection = null)
    {
        IQueryable<TEntity> query = Table;

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
    }

    public async Task<IEnumerable<TEntity>?> GetAllByExpression(Expression<Func<TEntity, bool>> toCheck)
    {
        var entities = await Table.Where(toCheck).ToListAsync();

        return entities;
    }

    public async Task<IEnumerable<TEntity>?> GetAllByExpressionWithIncludes(Expression<Func<TEntity, bool>> toCheck)
    {
        var entityType = _context.Model.FindEntityType(typeof(TEntity));

        var navigationProperties = entityType.GetNavigations();
        var query = Table.AsQueryable();

        foreach (var navigationProperty in navigationProperties)
            query = query.Include(navigationProperty.Name);

        query = query.Where(toCheck);

        var entities = await query.ToListAsync();

        return entities;
    }

    public IEnumerable<TEntity> Paging(int pageSize, int pageNumber, Expression<Func<TEntity, object>>[]? includes = null,
        Expression<Func<TEntity, bool>>[]? conditions = null, Func<TEntity, object>? orderBy = null, OrderByDirectionEnum? orderDirection = null)
    {
        IQueryable<TEntity> query = Table;

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
    }

    public IEnumerable<TEntity> PagingWithIncludes(int pageSize, int pageNumber, Expression<Func<TEntity, object>>[]? includes = null,
        Expression<Func<TEntity, bool>>[]? conditions = null, Func<TEntity, object>? orderBy = null, OrderByDirectionEnum? orderDirection = null)
    {
        var entityType = _context.Model.FindEntityType(typeof(TEntity));
        var navigationProperties = entityType.GetNavigations();

        IQueryable<TEntity> query = Table;

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
    }

    public async Task<int> CountByConditionAsync(Expression<Func<TEntity, bool>>[] conditions)
    {
        IQueryable<TEntity> query = Table;

        if (conditions.Any())
            foreach (var condition in conditions)
                query = query.Where(condition);

        return await query.CountAsync();
    }

    public int Count()
    {
        return Table.Count();
    }

    public async Task<int> CountAsync()
    {
        return await Table.CountAsync();
    }

    public bool IdExists(TId id)
    {
        return Table.Any(CreateEqualityExpressionForId(id));
    }

    public virtual async Task<bool> IdExistsAsync(TId id)
    {
        return await Table.AnyAsync(CreateEqualityExpressionForId(id));
    }

    public virtual TEntity? GetById(TId id)
    {
        var entity = Table.Find(id);

        return entity;
    }

    public virtual IEnumerable<TEntity?> GetManyByIds(IEnumerable<TId> ids)
    {
        var entities = Table.Where(x => ids.Contains(x.Id));

        return entities;
    }

    public virtual async Task<TEntity?> GetByIdAsync(TId id)
    {
        var entity = await Table.FindAsync(id);

        return entity;
    }

    public virtual async Task<TEntity?> GetByIdWithIncludesAsync(TId id)
    {
        var entityType = _context.Model.FindEntityType(typeof(TEntity));

        var navigationProperties = entityType.GetNavigations();
        var query = Table.AsQueryable();

        foreach (var navigationProperty in navigationProperties)
            query = query.Include(navigationProperty.Name);

        var entity = await query.FirstOrDefaultAsync(CreateEqualityExpressionForId(id));

        return entity;
    }

    public virtual async Task<IEnumerable<TEntity?>> GetManyByIdsAsync(IEnumerable<TId> ids)
    {
        var entity = await Table.Where(x => ids.Contains(x.Id)).ToListAsync();

        return entity;
    }

    public virtual async Task<TEntity?> GetByIdAsync(TId id, Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = Table;

        if (includes.Any())
            foreach (var include in includes)
                query = query.Include(include);

        query = query.AsQueryable();

        var entity = await query.FirstOrDefaultAsync(CreateEqualityExpressionForId(id));

        return entity;
    }

    public virtual async Task<bool> SomethingExists(Expression<Func<TEntity, bool>> toCheck)
    {
        return await Table.AnyAsync(toCheck);
    }

    public virtual async Task<TEntity?> GetEntityByExpression(Expression<Func<TEntity, bool>> toCheck)
    {
        var entity = await Table.FirstOrDefaultAsync(toCheck);

        return entity;
    }

    private Expression<Func<TEntity, bool>> CreateEqualityExpressionForId(TId id)
    {
        var lambdaParam = Expression.Parameter(typeof(TEntity));

        var leftExpression = Expression.PropertyOrField(lambdaParam, "Id");

        var idValue = Convert.ChangeType(id, typeof(TId));

        Expression<Func<object>> closure = () => idValue;
        var rightExpression = Expression.Convert(closure.Body, leftExpression.Type);

        var lambdaBody = Expression.Equal(leftExpression, rightExpression);

        return Expression.Lambda<Func<TEntity, bool>>(lambdaBody, lambdaParam);
    }
}

public class UpdateRepository<TId, TEntity> : ReadRepository<TId, TEntity>, IUpdateRepository<TId, TEntity>
    where TEntity : class, IEntity<TId>
{
    public UpdateRepository(BdtDbContext context, IMapper mapper)
        : base(context, mapper)
    { }

    public bool Update(TEntity entity)
    {
        using var transaction = _context.Database.BeginTransaction();
        try
        {
            var tracking = _context.ChangeTracker.Entries<TEntity>().Any(x => x.Entity.Id.Equals(entity.Id));
            if (!tracking)
            {
                Table.Update(entity);
            }

            bool updated = _context.SaveChanges() > 0;

            transaction.Commit();

            Table.Entry(entity).State = EntityState.Detached;
            return updated;
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            throw;
        }
        //Table.Attach(obj);
    }

    public bool UpdateMany(IEnumerable<TEntity> entities)
    {
        using var transaction = _context.Database.BeginTransaction();
        try
        {
            foreach (var entity in entities)
            {
                var tracking = _context.ChangeTracker.Entries<TEntity>().Any(x => x.Entity.Id.Equals(entity.Id));
                if (tracking)
                {
                    return false;
                }
            }

            Table.UpdateRange(entities);

            bool updated = _context.SaveChanges() > 0;

            transaction.Commit();
            foreach (var entity in entities)
            {
                Table.Entry(entity).State = EntityState.Detached;
            }

            return updated;
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            throw;
        }
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
    public CreateRepository(BdtDbContext context, IMapper mapper)
        : base(context, mapper)
    { }

    public TEntity Insert(TEntity entity)
    {
        Table.Add(entity);

        return entity;
    }

    public async Task<TEntity> InsertAsync(TEntity entity)
    {
        await Table.AddAsync(entity);

        if (!await SaveAsync())
            throw new Exception("Could not be added to database.");

        var dto = await GetByIdWithIncludesAsync(entity.Id)
            ?? throw new Exception("Entity was not added to database.");

        return dto;
    }

    public IEnumerable<TEntity> InsertMany(IEnumerable<TEntity> entities)
    {
        Table.AddRange(entities);

        if (!(_context.SaveChanges() > 0))
            throw new Exception("Failed to create new entities.");

        return entities;
    }

    public async Task<IEnumerable<TEntity>> InsertManyAsync(IEnumerable<TEntity> entities)
    {
        await Table.AddRangeAsync(entities);

        if (!await SaveAsync())
            throw new Exception("Failed to create new entities.");

        return entities;
    }

    public bool InsertOrUpdateMany(IEnumerable<TEntity> entities)
    {
        using var transaction = _context.Database.BeginTransaction();

        PropertyInfo propInfo = entities.First().GetType().GetProperty("Id");
        var type = propInfo.PropertyType;
        bool isLong = type.FullName == "System.Int64";
        try
        {
            Table.UpdateRange(entities);

            bool updated = _context.SaveChanges() > 0;

            transaction.Commit();

            return updated;
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            throw;
        }
    }
}

public class DeleteRepository<TId, TEntity> : CreateRepository<TId, TEntity>, IDeleteRepository<TId, TEntity>
    where TEntity : class, IEntity<TId>
{
    public DeleteRepository(BdtDbContext context, IMapper mapper)
        : base(context, mapper)
    { }

    public bool Delete(TEntity entity)
    {
        if (entity is not IEntitySoftDelete)
        {
            Table.Remove(entity);
            return Save();
        }

        if (entity is IEntitySoftDelete softDeleteEntity)
        {
            softDeleteEntity.IsDeleted = true;
        }

        var updated = Update(entity);
        return updated;
    }

    public bool DeleteMany(IEnumerable<TEntity> entities)
    {
        if (entities is IEnumerable<IEntitySoftDelete> softDeleteEntities)
        {
            foreach (var softDeleteEntity in softDeleteEntities)
            {
                softDeleteEntity.IsDeleted = true;
            }
            var updated = UpdateMany(entities);
            return updated;
        }

        Table.RemoveRange(entities);
        return Save();
    }

    public async Task<bool> DeleteAsync(TEntity entity)
    {
        if (entity is not IEntitySoftDelete)
        {
            await Task.Run(() =>
            {
                Table.Remove(entity);
            });

            return await SaveAsync();
        }

        if (entity is IEntitySoftDelete softDeleteEntity)
        {
            softDeleteEntity.IsDeleted = true;
        }

        var updated = Update(entity);
        return updated;
    }

    public async Task<bool> DeleteByIdAsync(TId id)
    {
        TEntity existing = await Table.FindAsync(id) ??
            throw new Exception($"ID {id} not found");

        if (existing is not IEntitySoftDelete)
        {
            Table.Remove(existing);

            return await SaveAsync();
        }

        if (existing is IEntitySoftDelete softDeleteEntity)
        {
            softDeleteEntity.IsDeleted = true;

        }

        var updated = Update(existing);
        return updated;
    }

    public async Task<bool> DeleteManyAsync(IEnumerable<TEntity> entities)
    {
        if (entities is not IEnumerable<IEntitySoftDelete>)
        {
            await Task.Run(() => { Table.RemoveRange(entities); });
            return await SaveAsync();
        }

        if (entities is IEnumerable<IEntitySoftDelete> softDeleteEntities)
        {
            foreach (var softDeleteEntity in softDeleteEntities)
            {
                softDeleteEntity.IsDeleted = true;
            }
        }

        var updated = UpdateMany(entities);
        return updated;
    }
}