using BdtDomain.Entities;
using BdtDomain.Enums;
using BdtDomain.Dtos;
using System.Linq.Expressions;
using BdtDomain.Repository;
using AutoMapper;

namespace BdtApplication.ApiServices.Generic;

public class GenericService<TId, TEntity, TDto> : IGenericService<TId, TEntity, TDto>
    where TEntity : class, IEntity<TId>
    where TDto : class, IBaseDto<TId>
{
    protected readonly IReadRepository<TId, TEntity> _repository;
    protected readonly IMapper _mapper;

    public GenericService(IReadRepository<TId, TEntity> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public virtual int Count()
    {
        var count = _repository.Count();
        return count;
    }

    public virtual async Task<int> CountAsync()
    {
        var count = await _repository.CountAsync();
        return count;
    }

    public virtual async Task<int> CountByConditionAsync(Expression<Func<TEntity, bool>>[] conditions)
    {
        var count = await _repository.CountByConditionAsync(conditions);
        return count;
    }

    public virtual IEnumerable<TDto> GetAll()
    {
        var entities = _repository.GetAll();
        var dtos = _mapper.Map<IEnumerable<TDto>>(entities);
        return dtos;
    }

    public virtual async Task<IEnumerable<TDto>> GetAllAsync()
    {
        var entities = await _repository.GetAllAsync();
        var dtos = _mapper.Map<IEnumerable<TDto>>(entities);
        return dtos;
    }

    public virtual async Task<IEnumerable<TDto>> GetAllAsync(Expression<Func<TEntity, object>>[]? includes = null,
        Expression<Func<TEntity, bool>>[]? conditions = null, Func<TEntity, object>? orderBy = null, OrderByDirectionEnum? orderDirection = null)
    {
        var entities = await _repository.GetAllAsync(includes, conditions, orderBy, orderDirection);

        var dtos = _mapper.Map<IEnumerable<TDto>>(entities);
        return dtos;
    }

    public virtual async Task<IEnumerable<TDto>?> GetAllByExpression(Expression<Func<TEntity, bool>> toCheck)
    {
        var entities = await _repository.GetAllByExpression(toCheck);
        var dtos = _mapper.Map<IEnumerable<TDto>>(entities);
        return dtos;
    }

    public virtual async Task<IEnumerable<TDto>?> GetAllByExpressionWithIncludes(Expression<Func<TEntity, bool>> toCheck)
    {
        var entities = await _repository.GetAllByExpressionWithIncludes(toCheck);
        var dtos = _mapper.Map<IEnumerable<TDto>>(entities);
        return dtos;
    }

    public virtual async Task<IEnumerable<TDto>> GetAllWithIncludesAsync()
    {
        var entities = await _repository.GetAllWithIncludesAsync();
        var dtos = _mapper.Map<IEnumerable<TDto>>(entities);
        return dtos;
    }

    public virtual TDto? GetById(TId id)
    {
        var entity = _repository.GetById(id);
        var dto = _mapper.Map<TDto>(entity);
        return dto;
    }

    public virtual async Task<TDto?> GetByIdAsync(TId id)
    {
        var entity = await _repository.GetByIdAsync(id);
        var dto = _mapper.Map<TDto>(entity);
        return dto;
    }

    public virtual async Task<TDto?> GetByIdAsync(TId id, Expression<Func<TEntity, object>>[] includes)
    {
        var entity = await _repository.GetByIdAsync(id, includes);
        var dto = _mapper.Map<TDto>(entity);
        return dto;
    }

    public virtual async Task<TDto?> GetByIdWithIncludesAsync(TId id)
    {
        var entity = await _repository.GetByIdWithIncludesAsync(id);
        var dto = _mapper.Map<TDto>(entity);
        return dto;
    }

    public virtual async Task<TDto?> GetEntityByExpression(Expression<Func<TEntity, bool>> toCheck)
    {
        var entity = await _repository.GetEntityByExpression(toCheck);
        var dto = _mapper.Map<TDto>(entity);
        return dto;
    }

    public virtual IEnumerable<TDto?> GetManyByIds(IEnumerable<TId> ids)
    {
        var entities = _repository.GetManyByIds(ids);
        var dtos = _mapper.Map<IEnumerable<TDto>>(entities);
        return dtos;
    }

    public virtual async Task<IEnumerable<TDto?>> GetManyByIdsAsync(IEnumerable<TId> ids)
    {
        var entities = await _repository.GetManyByIdsAsync(ids);
        var dtos = _mapper.Map<IEnumerable<TDto>>(entities);
        return dtos;
    }

    public virtual bool IdExists(TId id)
    {
        var exists = _repository.IdExists(id);
        return exists;
    }

    public virtual async Task<bool> IdExistsAsync(TId id)
    {
        var exists = await _repository.IdExistsAsync(id);
        return exists;
    }

    public virtual IEnumerable<TDto> Paging(int pageSize, int pageNumber, Expression<Func<TEntity, object>>[]? includes = null, Expression<Func<TEntity, bool>>[]? conditions = null, Func<TEntity, object>? orderBy = null, OrderByDirectionEnum? orderDirection = null)
    {
        var entities = _repository.Paging(pageSize, pageNumber, includes, conditions, orderBy, orderDirection);
        var dtos = _mapper.Map<IEnumerable<TDto>>(entities);
        return dtos;
    }

    public virtual IEnumerable<TDto> PagingWithIncludes(int pageSize, int pageNumber, Expression<Func<TEntity, object>>[]? includes = null, Expression<Func<TEntity, bool>>[]? conditions = null, Func<TEntity, object>? orderBy = null, OrderByDirectionEnum? orderDirection = null)
    {
        var entities = _repository.PagingWithIncludes(pageSize, pageNumber, includes, conditions, orderBy, orderDirection);
        var dtos = _mapper.Map<IEnumerable<TDto>>(entities);
        return dtos;
    }

    public virtual async Task<bool> SomethingExists(Expression<Func<TEntity, bool>> toCheck)
    {
        var somethingExists = await _repository.SomethingExists(toCheck);
        return somethingExists;
    }
}

public class GenericService<TId, TEntity, TDto, TUpdateDto> : GenericService<TId, TEntity, TDto>, IGenericService<TId, TEntity, TDto, TUpdateDto>
    where TEntity : class, IEntity<TId>
    where TDto : class, IBaseDto<TId>
    where TUpdateDto : class, IBaseDto<TId>
{
    protected new readonly IUpdateRepository<TId, TEntity> _repository;

    public GenericService(IUpdateRepository<TId, TEntity> repository, IMapper mapper)
        : base(repository, mapper)
    {
        _repository = repository;
    }

    public virtual bool Save()
    {
        var saved = _repository.Save();
        return saved;
    }

    public virtual async Task<bool> SaveAsync()
    {
        var saved = await _repository.SaveAsync();
        return saved;
    }

    public virtual bool Update(TUpdateDto obj)
    {
        var entity = _mapper.Map<TEntity>(obj);
        var updated = _repository.Update(entity);
        return updated;
    }

    public virtual bool UpdateMany(IEnumerable<TUpdateDto> objs)
    {
        var entities = _mapper.Map<IEnumerable<TEntity>>(objs);
        var updated = _repository.UpdateMany(entities);
        return updated;
    }
}

public class GenericService<TId, TEntity, TDto, TUpdateDto, TCreateDto> : GenericService<TId, TEntity, TDto, TUpdateDto>, IGenericService<TId, TEntity, TDto, TUpdateDto, TCreateDto>
    where TEntity : class, IEntity<TId>
    where TDto : class, IBaseDto<TId>
    where TUpdateDto : class, IBaseDto<TId>
{
    protected new readonly ICreateRepository<TId, TEntity> _repository;

    public GenericService(ICreateRepository<TId, TEntity> repository, IMapper mapper)
        : base(repository, mapper)
    {
        _repository = repository;
    }

    public virtual TDto Insert(TCreateDto obj)
    {
        var entity = _mapper.Map<TEntity>(obj);
        var result = _repository.Insert(entity);
        return _mapper.Map<TDto>(result);
    }

    public virtual async Task<TDto> InsertAsync(TCreateDto obj)
    {
        var entity = _mapper.Map<TEntity>(obj);
        var result = _repository.InsertAsync(entity);
        return _mapper.Map<TDto>(result);
    }

    public virtual IEnumerable<TDto> InsertMany(IEnumerable<TCreateDto> objs)
    {
        var entities = _mapper.Map<IEnumerable<TEntity>>(objs);
        var results = _repository.InsertMany(entities);
        return _mapper.Map<IEnumerable<TDto>>(results);
    }

    public virtual async Task<IEnumerable<TDto>> InsertManyAsync(IEnumerable<TCreateDto> objs)
    {
        var entities = _mapper.Map<IEnumerable<TEntity>>(objs);
        var results = await _repository.InsertManyAsync(entities);
        return _mapper.Map<IEnumerable<TDto>>(results);
    }

    public virtual bool InsertOrUpdateMany(IEnumerable<TCreateDto> objs)
    {
        var entities = _mapper.Map<IEnumerable<TEntity>>(objs);
        var isSuccessful = _repository.InsertOrUpdateMany(entities);
        return isSuccessful;
    }
}

public class GenericService<TId, TEntity, TDto, TUpdateDto, TCreateDto, TDeleteDto> : GenericService<TId, TEntity, TDto, TUpdateDto, TCreateDto>, IGenericService<TId, TEntity, TDto, TUpdateDto, TCreateDto, TDeleteDto>
    where TEntity : class, IEntity<TId>
    where TDto : class, IBaseDto<TId>
    where TUpdateDto : class, IBaseDto<TId>
    where TDeleteDto : class, IBaseDto<TId>
{
    protected new readonly IDeleteRepository<TId, TEntity> _repository;

    public GenericService(IDeleteRepository<TId, TEntity> repository, IMapper mapper)
        : base(repository, mapper)
    {
        _repository = repository;
    }

    public virtual bool Delete(TDeleteDto obj)
    {
        var entity = _mapper.Map<TEntity>(obj);
        var deleted = _repository.Delete(entity);
        return deleted;
    }

    public virtual bool DeleteMany(IEnumerable<TDeleteDto> objs)
    {
        var entities = _mapper.Map<IEnumerable<TEntity>>(objs);
        var deleted = _repository.DeleteMany(entities);
        return deleted;
    }

    public virtual async Task<bool> DeleteByIdAsync(TId id)
    {
        var deleted = await _repository.DeleteByIdAsync(id);
        return deleted;
    }

    public virtual Task<bool> DeleteAsync(TDeleteDto obj)
    {
        var deleted = _repository.DeleteByIdAsync(obj.Id);
        return deleted;
    }
}