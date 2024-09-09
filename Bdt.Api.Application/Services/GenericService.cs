using AutoMapper;
using Bdt.Api.Application.Services.Interfaces;
using Bdt.Api.Infrastructure.Repositories.Interfaces;
using Bdt.Api.Domain.Entities;
using Bdt.Shared.Dtos;
using Bdt.Shared.Enums;
using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using Bdt.Api.Infrastructure.Exceptions.Api;
using Bdt.Api.Infrastructure.Exceptions.Database;

namespace Bdt.Api.Application.Services;

public class GenericService<TId, TEntity, TDto> : IGenericService<TId, TEntity, TDto>
    where TEntity : class, IEntity<TId>
    where TDto : class, IBaseDto<TId>
{
    protected readonly IReadRepository<TId, TEntity> _repository;
    protected readonly IMapper _mapper;
    protected readonly ILogger _logger;

    public GenericService(IReadRepository<TId, TEntity> repository, IMapper mapper, ILogger<GenericService<TId, TEntity, TDto>> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public virtual int Count()
    {
        try
        {
            var count = _repository.Count();
            return count;
        }
        catch (Exception ex)
        {
            throw BuildExceptionToThrow(ex);
        }
    }

    public virtual async Task<int> CountAsync()
    {
        try
        {
            var count = await _repository.CountAsync();
            return count;
        }
        catch (Exception ex)
        {
            throw BuildExceptionToThrow(ex);
        }
    }

    public virtual async Task<int> CountByConditionAsync(Expression<Func<TEntity, bool>>[] conditions)
    {
        try
        {
            var count = await _repository.CountByConditionAsync(conditions);
            return count;
        }
        catch (Exception ex)
        {
            throw BuildExceptionToThrow(ex);
        }
    }

    public virtual IEnumerable<TDto> GetAll()
    {
        try
        {
            var entities = _repository.GetAll();
            var dtos = _mapper.Map<IEnumerable<TDto>>(entities);
            return dtos;
        }
        catch (Exception ex)
        {
            throw BuildExceptionToThrow(ex);
        }
    }

    public virtual async Task<IEnumerable<TDto>> GetAllAsync()
    {
        try
        {
            var entities = await _repository.GetAllAsync();
            var dtos = _mapper.Map<IEnumerable<TDto>>(entities);
            return dtos;
        }
        catch (Exception ex)
        {
            throw BuildExceptionToThrow(ex);
        }
    }

    public virtual async Task<IEnumerable<TDto>> GetAllAsync(Expression<Func<TEntity, object>>[]? includes = null,
        Expression<Func<TEntity, bool>>[]? conditions = null, Func<TEntity, object>? orderBy = null, OrderByDirectionEnum? orderDirection = null)
    {
        try
        {
            var entities = await _repository.GetAllAsync(includes, conditions, orderBy, orderDirection);

            var dtos = _mapper.Map<IEnumerable<TDto>>(entities);
            return dtos;
        }
        catch (Exception ex)
        {
            throw BuildExceptionToThrow(ex);
        }
    }

    public virtual async Task<IEnumerable<TDto>?> GetAllByExpression(Expression<Func<TEntity, bool>> toCheck)
    {
        try
        {
            var entities = await _repository.GetAllByExpression(toCheck);
            var dtos = _mapper.Map<IEnumerable<TDto>>(entities);
            return dtos;
        }
        catch (Exception ex)
        {
            throw BuildExceptionToThrow(ex);
        }
    }

    public virtual async Task<IEnumerable<TDto>?> GetAllByExpressionWithIncludes(Expression<Func<TEntity, bool>> toCheck)
    {
        try
        {
            var entities = await _repository.GetAllByExpressionWithIncludes(toCheck);
            var dtos = _mapper.Map<IEnumerable<TDto>>(entities);
            return dtos;
        }
        catch (Exception ex)
        {
            throw BuildExceptionToThrow(ex);
        }
    }

    public virtual async Task<IEnumerable<TDto>> GetAllWithIncludesAsync()
    {
        try
        {
            var entities = await _repository.GetAllWithIncludesAsync();
            var dtos = _mapper.Map<IEnumerable<TDto>>(entities);
            return dtos;
        }
        catch (Exception ex)
        {
            throw BuildExceptionToThrow(ex);
        }
    }

    public virtual TDto? GetById(TId id)
    {
        try
        {
            var entity = _repository.GetById(id);
            var dto = _mapper.Map<TDto>(entity);
            return dto;
        }
        catch (Exception ex)
        {
            throw BuildExceptionToThrow(ex);
        }
    }

    public virtual async Task<TDto?> GetByIdAsync(TId id)
    {
        try
        {
            var entity = await _repository.GetByIdAsync(id);
            var dto = _mapper.Map<TDto>(entity);
            return dto;
        }
        catch (Exception ex)
        {
            throw BuildExceptionToThrow(ex);
        }
    }

    public virtual async Task<TDto?> GetByIdAsync(TId id, Expression<Func<TEntity, object>>[] includes)
    {
        try
        {
            var entity = await _repository.GetByIdAsync(id, includes);
            var dto = _mapper.Map<TDto>(entity);
            return dto;
        }
        catch (Exception ex)
        {
            throw BuildExceptionToThrow(ex);
        }
    }

    public virtual async Task<TDto?> GetByIdWithIncludesAsync(TId id)
    {
        try
        {
            var entity = await _repository.GetByIdWithIncludesAsync(id);
            var dto = _mapper.Map<TDto>(entity);
            return dto;
        }
        catch (Exception ex)
        {
            throw BuildExceptionToThrow(ex);
        }
    }

    public virtual async Task<TDto?> GetEntityByExpression(Expression<Func<TEntity, bool>> toCheck)
    {
        try
        {
            var entity = await _repository.GetEntityByExpression(toCheck);
            var dto = _mapper.Map<TDto>(entity);
            return dto;
        }
        catch (Exception ex)
        {
            throw BuildExceptionToThrow(ex);
        }
    }

    public virtual IEnumerable<TDto?> GetManyByIds(IEnumerable<TId> ids)
    {
        try
        {
            var entities = _repository.GetManyByIds(ids);
            var dtos = _mapper.Map<IEnumerable<TDto>>(entities);
            return dtos;
        }
        catch (Exception ex)
        {
            throw BuildExceptionToThrow(ex);
        }
    }

    public virtual async Task<IEnumerable<TDto?>> GetManyByIdsAsync(IEnumerable<TId> ids)
    {
        try
        {
            var entities = await _repository.GetManyByIdsAsync(ids);
            var dtos = _mapper.Map<IEnumerable<TDto>>(entities);
            return dtos;
        }
        catch (Exception ex)
        {
            throw BuildExceptionToThrow(ex);
        }
    }

    public virtual bool IdExists(TId id)
    {
        try
        {
            var exists = _repository.IdExists(id);
            return exists;
        }
        catch (Exception ex)
        {
            throw BuildExceptionToThrow(ex);
        }
    }

    public virtual async Task<bool> IdExistsAsync(TId id)
    {
        try
        {
            var exists = await _repository.IdExistsAsync(id);
            return exists;
        }
        catch (Exception ex)
        {
            throw BuildExceptionToThrow(ex);
        }
    }

    public virtual IEnumerable<TDto> Paging(int pageSize, int pageNumber, Expression<Func<TEntity, object>>[]? includes = null, Expression<Func<TEntity, bool>>[]? conditions = null, Func<TEntity, object>? orderBy = null, OrderByDirectionEnum? orderDirection = null)
    {
        try
        {
            var entities = _repository.Paging(pageSize, pageNumber, includes, conditions, orderBy, orderDirection);
            var dtos = _mapper.Map<IEnumerable<TDto>>(entities);
            return dtos;
        }
        catch (Exception ex)
        {
            throw BuildExceptionToThrow(ex);
        }
    }

    public virtual IEnumerable<TDto> PagingWithIncludes(int pageSize, int pageNumber, Expression<Func<TEntity, object>>[]? includes = null, Expression<Func<TEntity, bool>>[]? conditions = null, Func<TEntity, object>? orderBy = null, OrderByDirectionEnum? orderDirection = null)
    {
        try
        {
            var entities = _repository.PagingWithIncludes(pageSize, pageNumber, includes, conditions, orderBy, orderDirection);
            var dtos = _mapper.Map<IEnumerable<TDto>>(entities);
            return dtos;
        }
        catch (Exception ex)
        {
            throw BuildExceptionToThrow(ex);
        }
    }

    public virtual async Task<bool> SomethingExists(Expression<Func<TEntity, bool>> toCheck)
    {
        try
        {
            var somethingExists = await _repository.SomethingExists(toCheck);
            return somethingExists;
        }
        catch (Exception ex)
        {
            throw BuildExceptionToThrow(ex);
        }
    }

    public Exception BuildExceptionToThrow(Exception ex, string? customUserMessage = null)
    {
        var entityName = typeof(TEntity).Name.Replace("Entity", "(s)");

        if(ex is AutoMapperMappingException)
        {
            var entity = typeof(TEntity).Name ?? "entities";
            _logger.LogError(ex, $"Failed to map between {nameof(TEntity)} and {nameof(TDto)}.");
            return new UserFriendlyException($"Failed to get all {entity}s.", ex);
        }
        if (ex is DbOperationFailedException operationFailedException)
        {
            var defaultMessage = $"Failed to get all {entityName}s.";
            var message = customUserMessage ?? defaultMessage;
            _logger.LogError(ex, operationFailedException.Message );
            return new UserFriendlyException(message, ex);
        }
        else
        {
            var entity = typeof(TEntity).Name ?? "entities";
            var defaultMessage = $"An unknown error has occured during operation with {entityName}.";
            var message = customUserMessage ?? defaultMessage;
            _logger.LogError(ex, message);
            return new UserFriendlyException(message, ex);
        }
    }
}

public class GenericService<TId, TEntity, TDto, TUpdateDto> : GenericService<TId, TEntity, TDto>, IGenericService<TId, TEntity, TDto, TUpdateDto>
    where TEntity : class, IEntity<TId>
    where TDto : class, IBaseDto<TId>
    where TUpdateDto : class, IBaseDto<TId>
{
    protected new readonly IUpdateRepository<TId, TEntity> _repository;

    public GenericService(IUpdateRepository<TId, TEntity> repository, IMapper mapper, ILogger<GenericService<TId, TEntity, TDto, TUpdateDto>> logger)
        : base(repository, mapper, logger)
    {
        _repository = repository;
    }

    public virtual bool Save()
    {
        try
        {
        }
        catch (Exception ex)
        {
            throw BuildExceptionToThrow(ex);
        }
        var saved = _repository.Save();
        return saved;
    }

    public virtual async Task<bool> SaveAsync()
    {
        try
        {
            var saved = await _repository.SaveAsync();
            return saved;
        }
        catch (Exception ex)
        {
            throw BuildExceptionToThrow(ex);
        }
    }

    public virtual bool Update(TUpdateDto obj)
    {
        try
        {
            var entity = _mapper.Map<TEntity>(obj);
            var updated = _repository.Update(entity);
            return updated;
        }
        catch (Exception ex)
        {
            throw BuildExceptionToThrow(ex);
        }
    }

    public virtual bool UpdateMany(IEnumerable<TUpdateDto> objs)
    {
        try
        {
            var entities = _mapper.Map<IEnumerable<TEntity>>(objs);
            var updated = _repository.UpdateMany(entities);
            return updated;
        }
        catch (Exception ex)
        {
            throw BuildExceptionToThrow(ex);
        }
    }
}

public class GenericService<TId, TEntity, TDto, TUpdateDto, TCreateDto> : GenericService<TId, TEntity, TDto, TUpdateDto>, IGenericService<TId, TEntity, TDto, TUpdateDto, TCreateDto>
    where TEntity : class, IEntity<TId>
    where TDto : class, IBaseDto<TId>
    where TUpdateDto : class, IBaseDto<TId>
{
    protected new readonly ICreateRepository<TId, TEntity> _repository;

    public GenericService(ICreateRepository<TId, TEntity> repository, IMapper mapper, ILogger<GenericService<TId, TEntity, TDto, TUpdateDto, TCreateDto>> logger)
        : base(repository, mapper, logger)
    {
        _repository = repository;
    }

    public virtual TDto Insert(TCreateDto obj)
    {
        try
        {
            var entity = _mapper.Map<TEntity>(obj);
            var result = _repository.Insert(entity);
            return _mapper.Map<TDto>(result);
        }
        catch (Exception ex)
        {
            throw BuildExceptionToThrow(ex);
        }
    }

    public virtual async Task<TDto> InsertAsync(TCreateDto obj)
    {
        try
        {
            var entity = _mapper.Map<TEntity>(obj);
            var result = await _repository.InsertAsync(entity);
            return _mapper.Map<TDto>(result);
        }
        catch (Exception ex)
        {
            throw BuildExceptionToThrow(ex);
        }
    }

    public virtual IEnumerable<TDto> InsertMany(IEnumerable<TCreateDto> objs)
    {
        try
        {
            var entities = _mapper.Map<IEnumerable<TEntity>>(objs);
            var results = _repository.InsertMany(entities);
            return _mapper.Map<IEnumerable<TDto>>(results);
        }
        catch (Exception ex)
        {
            throw BuildExceptionToThrow(ex);
        }
    }

    public virtual async Task<IEnumerable<TDto>> InsertManyAsync(IEnumerable<TCreateDto> objs)
    {
        try
        {
            var entities = _mapper.Map<IEnumerable<TEntity>>(objs);
            var results = await _repository.InsertManyAsync(entities);
            return _mapper.Map<IEnumerable<TDto>>(results);
        }
        catch (Exception ex)
        {
            throw BuildExceptionToThrow(ex);
        }
    }

    public virtual bool InsertOrUpdateMany(IEnumerable<TCreateDto> objs)
    {
        try
        {
            var entities = _mapper.Map<IEnumerable<TEntity>>(objs);
            var isSuccessful = _repository.InsertOrUpdateMany(entities);
            return isSuccessful;
        }
        catch (Exception ex)
        {
            throw BuildExceptionToThrow(ex);
        }
    }
}

public class GenericService<TId, TEntity, TDto, TUpdateDto, TCreateDto, TDeleteDto> : GenericService<TId, TEntity, TDto, TUpdateDto, TCreateDto>, IGenericService<TId, TEntity, TDto, TUpdateDto, TCreateDto, TDeleteDto>
    where TEntity : class, IEntity<TId>
    where TDto : class, IBaseDto<TId>
    where TUpdateDto : class, IBaseDto<TId>
    where TDeleteDto : class, IBaseDto<TId>
{
    protected new readonly IDeleteRepository<TId, TEntity> _repository;

    public GenericService(IDeleteRepository<TId, TEntity> repository, IMapper mapper, ILogger<GenericService<TId, TEntity, TDto, TUpdateDto, TCreateDto, TDeleteDto>> logger)
        : base(repository, mapper, logger)
    {
        _repository = repository;
    }

    public virtual bool Delete(TDeleteDto obj)
    {
        try
        {
            var entity = _mapper.Map<TEntity>(obj);
            var deleted = _repository.Delete(entity);
            return deleted;
        }
        catch (Exception ex)
        {
            throw BuildExceptionToThrow(ex);
        }
    }

    public virtual bool DeleteMany(IEnumerable<TDeleteDto> objs)
    {
        try
        {
            var entities = _mapper.Map<IEnumerable<TEntity>>(objs);
            var deleted = _repository.DeleteMany(entities);
            return deleted;
        }
        catch (Exception ex)
        {
            throw BuildExceptionToThrow(ex);
        }
    }

    public virtual async Task<bool> DeleteByIdAsync(TId id)
    {
        try
        {
            var deleted = await _repository.DeleteByIdAsync(id);
            return deleted;
        }
        catch (Exception ex)
        {
            throw BuildExceptionToThrow(ex);
        }
    }

    public virtual Task<bool> DeleteAsync(TDeleteDto obj)
    {
        try
        {
            var deleted = _repository.DeleteByIdAsync(obj.Id);
            return deleted;
        }
        catch (Exception ex)
        {
            throw BuildExceptionToThrow(ex);
        }
    }
}