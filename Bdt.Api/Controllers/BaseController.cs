using AutoMapper;
using Bdt.Api.Application.Services.Interfaces;
using Bdt.Api.Domain.Entities;
using Bdt.Api.Domain.Helpers;
using Bdt.Api.Infrastructure.Exceptions.Api;
using Bdt.Shared.Dtos;
using Bdt.Shared.Models.App;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Bdt.Api.Controllers;

//[Route("api/[controller]")]
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize]
public class BaseController<TId, TEntity, TDto> : ControllerBase
    where TEntity : class, IEntity<TId>
    where TDto : class, IBaseDto<TId>
{
    protected readonly IMapper _mapper;
    protected readonly IGenericService<TId, TEntity, TDto> _service;

    public BaseController(IMapper mapper, IGenericService<TId, TEntity, TDto> service)
    {
        _mapper = mapper;
        _service = service;
    }

    [HttpGet("GetAll")]
    public virtual async Task<ActionResult<IEnumerable<TDto>>> GetAll()
    {
        try
        {
            var entities = await _service.GetAllWithIncludesAsync();

            return Ok(ApiWrapper<IEnumerable<TDto>>.Success(entities));
        }
        catch (UserFriendlyException ex)
        {
            var error = ApiWrapper<IEnumerable<TDto>>.Failed(ex.Message);

            return BadRequest(error);
        }
        catch (Exception ex)
        {
            var error = ApiWrapper<IEnumerable<TDto>>.Failed($"An unknown error occurred when trying to retrieve from database: {ex.Message}");

            return BadRequest(error);
        }
    }

    [HttpGet("GetAll/{pageSize}/{pageNumber}")]
    public virtual async Task<ActionResult<IEnumerable<TDto>>> GetAll(int pageSize = 5, int pageNumber = 1)
    {
        if (pageNumber < 1 || pageSize < 1)
        {
            var badNumbers = ApiWrapper<IEnumerable<TDto>>.Failed("pageNumber and pageSize must be TIdegers greater than 1");

            return BadRequest(badNumbers);
        }

        try
        {
            var totalCount = await _service.CountAsync();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var metadata = new PaginationMetadata
            {
                TotalCount = totalCount,
                PageSize = pageSize,
                CurrentPage = pageNumber,
                TotalPages = totalPages
            };

            var entities = _service.PagingWithIncludes(pageSize, pageNumber);
            var dtos = _mapper.Map<IEnumerable<TDto>>(entities);

            return Ok(ApiWrapper<IEnumerable<TDto>, PaginationMetadata>.Success(dtos, metadata));
        }
        catch (UserFriendlyException ex)
        {
            var error = ApiWrapper<IEnumerable<TDto>>.Failed(ex.Message);

            return BadRequest(error);
        }
        catch (Exception ex)
        {
            var error = ApiWrapper<IEnumerable<TDto>, PaginationMetadata>.Failed($"An unknown error occurred when trying to retrieve from database: {ex.Message}");

            return BadRequest(error);
        }
    }

    [HttpPost("GetAllFiltered/{pageSize}/{pageNumber}")]
    public async Task<ActionResult<ApiWrapper<IEnumerable<TDto>>>> GetAllFiltered([FromBody] List<FilterModel>? filters, int pageSize = 5, int pageNumber = 1)
    {
        if ((pageNumber < 1 || pageSize < 1) && filters is null)
        {
            var badNumbers = ApiWrapper<IEnumerable<TDto>>.Failed("pageNumber and pageSize must be TIdegers greater than 1");

            return BadRequest(badNumbers);
        }

        try
        {
            int totalCount = 0;
            int totalPages = 0;
            IEnumerable<TDto> pagingRepo;
            PaginationMetadata metadata;

            if (filters != null && filters.Count > 0)
            {
                var conditions = ControllersHelpers<TEntity>.GetLamdas(filters);

                if (conditions is null)
                {
                    var noConditions = ApiWrapper<IEnumerable<TDto>>.Failed("FilterValue has to be of type decimal, string or DateTime. Also check FilterOperator");

                    return BadRequest(noConditions);
                }

                pagingRepo = _service.Paging(pageSize, pageNumber, conditions: conditions);

                totalCount = await _service.CountByConditionAsync(conditions);
                totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            }
            else
            {
                pagingRepo = _service.Paging(pageSize, pageNumber);

                totalCount = await _service.CountAsync();
                totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            }

            metadata = new PaginationMetadata
            {
                TotalCount = totalCount,
                PageSize = pageSize,
                CurrentPage = pageNumber,
                TotalPages = totalPages
            };

            var response = ApiWrapper<IEnumerable<TDto>, PaginationMetadata>.Success(pagingRepo, metadata);

            return Ok(response);
        }
        catch (UserFriendlyException ex)
        {
            var error = ApiWrapper<IEnumerable<TDto>>.Failed(ex.Message);

            return BadRequest(error);
        }
        catch (Exception ex)
        {
            var error = ApiWrapper<IEnumerable<TId>>.Failed($"An unknown error occurred when trying to fetch data from database: {ex.Message}");

            return BadRequest(error);
        }
    }

    [HttpGet("GetAllWithoutRelatedEntities")]
    public virtual async Task<ActionResult<IEnumerable<TDto>>> GetAllWithoutRelatedEntities()
    {
        try
        {
            var entities = await _service.GetAllAsync();
            var dtos = _mapper.Map<IEnumerable<TDto>>(entities);

            return Ok(ApiWrapper<IEnumerable<TDto>>.Success(dtos));
        }
        catch (UserFriendlyException ex)
        {
            var error = ApiWrapper<IEnumerable<TDto>>.Failed(ex.Message);

            return BadRequest(error);
        }
        catch (Exception ex)
        {
            var error = ApiWrapper<IEnumerable<TDto>>.Failed($"An unknown error occurred when trying to retrieve from database: {ex.Message}");

            return BadRequest(error);
        }
    }

    [HttpGet("GetAllWithoutRelatedEntities/{pageSize}/{pageNumber}")]
    public virtual async Task<ActionResult<IEnumerable<TDto>>> GetAllWithoutRelatedEntities(int pageSize = 5, int pageNumber = 1)
    {
        if (pageNumber < 1 || pageSize < 1)
        {
            var badNumbers = ApiWrapper<IEnumerable<TDto>>.Failed("pageNumber and pageSize must be TIdegers greater than 1");

            return BadRequest(badNumbers);
        }

        try
        {
            var totalCount = await _service.CountAsync();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var metadata = new PaginationMetadata
            {
                TotalCount = totalCount,
                PageSize = pageSize,
                CurrentPage = pageNumber,
                TotalPages = totalPages
            };

            var entities = _service.Paging(pageSize, pageNumber);
            var dtos = _mapper.Map<IEnumerable<TDto>>(entities);

            return Ok(ApiWrapper<IEnumerable<TDto>, PaginationMetadata>.Success(dtos, metadata));
        }
        catch (UserFriendlyException ex)
        {
            var error = ApiWrapper<IEnumerable<TDto>>.Failed(ex.Message);

            return BadRequest(error);
        }
        catch (Exception ex)
        {
            var error = ApiWrapper<IEnumerable<TDto>>.Failed($"An unknown error occurred when trying to retrieve from database: {ex.Message}");

            return BadRequest(error);
        }
    }

    [HttpGet("{id}", Name = "GetBy[controller]Id")]
    public virtual async Task<ActionResult<TDto>> GetById(TId id)
    {
        if (id is int intId && intId < 1 || id is long longId && longId < 1)
        {
            var badNumbers = ApiWrapper<IEnumerable<TDto>>.Failed("Id must be an TIdeger greater than zero");

            return BadRequest(badNumbers);
        }

        try
        {
            var entity = await _service.GetByIdWithIncludesAsync(id);

            if (entity is null)
            {
                var notFound = ApiWrapper<TDto>.Failed($"No entry with Id {id} exist in database.");

                return NotFound(notFound);
            }

            var dto = _mapper.Map<TDto>(entity);

            return Ok(ApiWrapper<TDto>.Success(dto));
        }
        catch (UserFriendlyException ex)
        {
            var error = ApiWrapper<IEnumerable<TDto>>.Failed(ex.Message);

            return BadRequest(error);
        }
        catch (Exception ex)
        {
            var error = ApiWrapper<IEnumerable<TDto>>.Failed($"An unknown error occurred when trying to retrieve from database: {ex.Message}");

            return BadRequest(error);
        }
    }

    [HttpGet("GetByIdWithoutRelatedEntities/{id}")]
    public virtual async Task<ActionResult<TDto>> GetByIdWithoutRelatedEntities(TId id)
    {
        if (id is int intId && intId < 1 || id is long longId && longId < 1)
        {
            var badNumbers = ApiWrapper<IEnumerable<TDto>>.Failed("Id must be an TIdeger greater than zero");

            return BadRequest(badNumbers);
        }

        try
        {
            var entity = await _service.GetByIdAsync(id);

            if (entity is null)
            {
                var notFound = ApiWrapper<TDto>.Failed($"No entry with Id {id} exist in database.");

                return NotFound(notFound);
            }

            var dto = _mapper.Map<TDto>(entity);

            return Ok(ApiWrapper<TDto>.Success(dto));
        }
        catch (UserFriendlyException ex)
        {
            var error = ApiWrapper<IEnumerable<TDto>>.Failed(ex.Message);

            return BadRequest(error);
        }
        catch (Exception ex)
        {
            var error = ApiWrapper<IEnumerable<TDto>>.Failed($"An unknown error occurred when trying to retrieve from database: {ex.Message}");

            return BadRequest(error);
        }
    }

    [HttpGet("CountAll")]
    public virtual async Task<ActionResult<ApiWrapper<int>>> CountAll()
    {
        try
        {
            var count = await _service.CountAsync();

            return ApiWrapper<int>.Success(count);
        }
        catch (UserFriendlyException ex)
        {
            var error = ApiWrapper<IEnumerable<TDto>>.Failed(ex.Message);

            return BadRequest(error);
        }
        catch (Exception ex)
        {
            var error = ApiWrapper<IEnumerable<int>>.Failed($"An unknown error occurred when trying to count: {ex.Message}");

            return BadRequest(error);
        }
    }
}

public class BaseController<TId, TEntity, TDto, TUpdateDto>
    : BaseController<TId, TEntity, TDto>
    where TEntity : class, IEntity<TId>
    where TDto : class, IBaseDto<TId>
    where TUpdateDto : class, IBaseDto<TId>
{
    protected readonly new IGenericService<TId, TEntity, TDto, TUpdateDto> _service;
    public BaseController(IMapper mapper, IGenericService<TId, TEntity, TDto, TUpdateDto> service)
        : base(mapper, service)
    {
        _service = service;
    }

    [HttpPatch("Update")]
    public virtual async Task<ActionResult<ApiWrapper<bool>>> Update(TUpdateDto updateDto)
    {
        try
        {
            var exists = await _service.IdExistsAsync(updateDto.Id);

            if (!exists)
            {
                var notFound = ApiWrapper<TEntity>.Failed($"No entry with Id {updateDto.Id} exist in database.");

                return NotFound(notFound);
            }

            if (!_service.Update(updateDto))
            {
                return BadRequest(ApiWrapper<TDto>.Failed("Could not update entity"));
            }

            return Ok(ApiWrapper<bool>.Success(true));
        }
        catch (UserFriendlyException ex)
        {
            var error = ApiWrapper<IEnumerable<TDto>>.Failed(ex.Message);

            return BadRequest(error);
        }
        catch (Exception ex)
        {
            var error = ApiWrapper<TDto>.Failed($"An unknown error occurred when trying to update: {ex.Message}");

            return BadRequest(error);
        }
    }

    [HttpPatch("UpdateMany")]
    public virtual async Task<ActionResult<bool>> UpdateMany(IEnumerable<TUpdateDto> updateDtos)
    {
        try
        {
            _service.UpdateMany(updateDtos);
            var saved = await _service.SaveAsync();

            return Ok(ApiWrapper<bool>.Success(saved));
        }
        catch (UserFriendlyException ex)
        {
            var error = ApiWrapper<IEnumerable<TDto>>.Failed(ex.Message);

            return BadRequest(error);
        }
        catch (Exception ex)
        {
            var error = ApiWrapper<IEnumerable<TDto>>.Failed($"An unknown error occurred when trying to update: {ex.Message}");

            return BadRequest(error);
        }
    }
}

public class BaseController<TId, TEntity, TDto, TUpdateDto, TCreateDto>
    : BaseController<TId, TEntity, TDto, TUpdateDto>
    where TEntity : class, IEntity<TId>
    where TDto : class, IBaseDto<TId>
    where TUpdateDto : class, IBaseDto<TId>
{
    protected readonly new IGenericService<TId, TEntity, TDto, TUpdateDto, TCreateDto> _service;
    public BaseController(IMapper mapper, IGenericService<TId, TEntity, TDto, TUpdateDto, TCreateDto> service)
        : base(mapper, service)
    {
        _service = service;
    }

    [HttpPost("Create")]
    public virtual async Task<ActionResult<ApiWrapper<TDto>>> Create(TCreateDto createDto)
    {
        try
        {
            if (createDto is IUserDto userDto)
            {
                string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(userId))
                    return BadRequest(ApiWrapper<TDto>.Failed("User identifier is missing."));

                userDto.UserId = userId;
            }

            var responseDto = await _service.InsertAsync(createDto);

            return Ok(ApiWrapper<TDto>.Success(responseDto));
        }
        catch (UserFriendlyException ex)
        {
            var error = ApiWrapper<IEnumerable<TDto>>.Failed(ex.Message);

            return BadRequest(error);
        }
        catch (Exception ex)
        {
            var error = ApiWrapper<TDto>.Failed($"An unknown error occurred during the create request: {ex.Message}");

            return BadRequest(error);
        }
    }

    [HttpPost("CreateMany")]
    public virtual async Task<ActionResult<IEnumerable<TDto>>> CreateMany(IEnumerable<TCreateDto> createDtos)
    {
        try
        {
            if (createDtos is IEnumerable<IUserEntity> userDtos)
            {
                string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(userId))
                    return BadRequest(ApiWrapper<IEnumerable<TDto>>.Failed("User identifier is missing."));

                foreach (var userDto in userDtos)
                    userDto.UserId = userId;
            }

            var responseDto = await _service.InsertManyAsync(createDtos);

            return Ok(ApiWrapper<IEnumerable<TDto>>.Success(responseDto));
        }
        catch (UserFriendlyException ex)
        {
            var error = ApiWrapper<IEnumerable<TDto>>.Failed(ex.Message);

            return BadRequest(error);
        }
        catch (Exception ex)
        {
            var error = ApiWrapper<IEnumerable<TDto>>.Failed($"An unknown error occurred during the create request: {ex.Message}");

            return BadRequest(error);
        }
    }
}

public class BaseController<TId, TEntity, TDto, TUpdateDto, TCreateDto, TDeleteDto>
    : BaseController<TId, TEntity, TDto, TUpdateDto, TCreateDto>
    where TEntity : class, IEntity<TId>
    where TDto : class, IBaseDto<TId>
    where TUpdateDto : class, IBaseDto<TId>
    where TDeleteDto : class, IBaseDto<TId>
{
    protected readonly new IGenericService<TId, TEntity, TDto, TUpdateDto, TCreateDto, TDeleteDto> _service;
    public BaseController(IMapper mapper, IGenericService<TId, TEntity, TDto, TUpdateDto, TCreateDto, TDeleteDto> service)
        : base(mapper, service)
    {
        _service = service;
    }

    [HttpDelete("Delete/{id}")]
    public virtual async Task<ActionResult<TDto>> Delete(TId id)
    {
        if (id is int intId && intId < 1 || id is long longId && longId < 1)
        {
            var badNumbers = ApiWrapper<TDto>.Failed("Id must be an integer greater than zero");

            return BadRequest(badNumbers);
        }

        var dto = await _service.GetByIdAsync(id);

        if (dto is null)
        {
            var notFound = ApiWrapper<TDto>.Failed($"No entry with Id {id} exist in database.");

            return NotFound(notFound);
        }

        try
        {
            var deleteDto = _mapper.Map<TDeleteDto>(dto);
            if (!await _service.DeleteAsync(deleteDto))
            {
                ApiWrapper<TDto>.Failed("Failed to delete from database");
            }

            return Ok(ApiWrapper<TDto>.Success(dto));
        }
        catch (UserFriendlyException ex)
        {
            var error = ApiWrapper<IEnumerable<TDto>>.Failed(ex.Message);

            return BadRequest(error);
        }
        catch (Exception ex)
        {
            var error = ApiWrapper<TDto>.Failed($"An unknown error occurred during the delete request: {ex.Message}");

            return BadRequest(error);
        }
    }

    [HttpDelete("DeleteMany")]
    public virtual async Task<ActionResult<IEnumerable<TDto>>> DeleteMany(IEnumerable<TId> ids)
    {
        try
        {
            var dtos = await _service.GetAllByExpressionWithIncludes(x => ids.Contains(x.Id));

            if (dtos is null)
            {
                var notFound = ApiWrapper<IEnumerable<TDto>>.Failed($"No entries were found for the provided ids.");

                return NotFound(notFound);
            }

            var deleteDtos = _mapper.Map<IEnumerable<TDeleteDto>>(dtos);
            _service.DeleteMany(deleteDtos);

            return Ok(ApiWrapper<IEnumerable<TDto>>.Success(dtos));
        }
        catch (UserFriendlyException ex)
        {
            var error = ApiWrapper<IEnumerable<TDto>>.Failed(ex.Message);

            return BadRequest(error);
        }
        catch (Exception ex)
        {
            var error = ApiWrapper<IEnumerable<TDto>>.Failed($"An unknown error occurred during the delete request: {ex.Message}");

            return BadRequest(error);
        }
    }
}