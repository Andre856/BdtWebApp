using AutoMapper;
using Bdt.Api.Application.Services.Interfaces;
using Bdt.Api.Controllers;
using Bdt.Api.Domain.Entities;
using Bdt.Api.Infrastructure.Exceptions.Api;
using Bdt.Shared.Dtos.Planner;
using Bdt.Shared.Dtos.Workouts;
using Bdt.Shared.Dtos.WorkoutValues;
using Bdt.Shared.Models.App;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Bdt.Api.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize]
public class WorkoutController : BaseController<Guid, WorkoutEntity, WorkoutDto, UpdateWorkoutDto, CreateWorkoutDto, DeleteWorkoutDto>
{
    private readonly IWorkoutService _workoutService;
    private readonly IWorkoutValuesService _workoutValuesService;

    public WorkoutController(IMapper mapper,
        IGenericService<Guid, WorkoutEntity, WorkoutDto, UpdateWorkoutDto, CreateWorkoutDto, DeleteWorkoutDto> service,
        IWorkoutService workoutService, IWorkoutValuesService workoutValuesService)
        : base(mapper, service)
    {
        _workoutService = workoutService;
        _workoutValuesService = workoutValuesService;
    }

    [HttpGet("GetUserWorkouts")]
    public async Task<ActionResult<ApiWrapper<IEnumerable<WorkoutDto>>>> GetAllByUserId()
    {
        string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
            return BadRequest(ApiWrapper<IEnumerable<WorkoutDto>>.Failed("User identifier is missing."));

        var workouts = await _workoutService.GetAllByUserId(userId);

        var userWorkoutsDto = _mapper.Map<IEnumerable<WorkoutDto>>(workouts);

        return Ok(ApiWrapper<IEnumerable<WorkoutDto>>.Success(userWorkoutsDto));
    }

    [HttpGet("GetUserWorkoutsLastMonth")]
    public async Task<ActionResult<ApiWrapper<IEnumerable<WorkoutDto>>>> GetUserWorkoutsLastMonth()
    {
        string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
            return BadRequest(ApiWrapper<IEnumerable<WorkoutDto>>.Failed("User identifier is missing."));

        var workouts = await _workoutService.GetLastMonthByUserId(userId);

        var userWorkoutsDto = _mapper.Map<IEnumerable<WorkoutDto>>(workouts);

        return Ok(ApiWrapper<IEnumerable<WorkoutDto>>.Success(userWorkoutsDto));
    }

    [HttpDelete("Delete/{id}")]
    public override async Task<ActionResult<WorkoutDto>> Delete(Guid id)
    {
        var workoutDto = await _service.GetByIdAsync(id);
        var workoutValuesDto = await _workoutValuesService.GetEntityByExpression(x => x.WorkoutId == id);

        if (workoutDto is null)
        {
            var notFound = ApiWrapper<WorkoutDto>.Failed($"No entry with Id {id} exist in database.");

            return NotFound(notFound);
        }

        if (workoutValuesDto is null)
        {
            var notFound = ApiWrapper<WorkoutDto>.Failed($"No entry with Id {id} exist in database.");

            return NotFound(notFound);
        }

        try
        {
            var deleteWorkoutValuesDto = _mapper.Map<DeleteWorkoutValuesDto>(workoutValuesDto);
            if (!await _workoutValuesService.DeleteAsync(deleteWorkoutValuesDto))
            {
                ApiWrapper<WorkoutDto>.Failed("Failed to delete from database");
            }

            var deleteWorkoutDto = _mapper.Map<DeleteWorkoutDto>(workoutDto);
            if (!await _service.DeleteAsync(deleteWorkoutDto))
            {
                ApiWrapper<WorkoutDto>.Failed("Failed to delete from database");
            }

            return Ok(ApiWrapper<WorkoutDto>.Success(workoutDto));
        }
        catch (UserFriendlyException ex)
        {
            var error = ApiWrapper<IEnumerable<PlannerDto>>.Failed(ex.Message);

            return BadRequest(error);
        }
        catch (Exception ex)
        {
            var error = ApiWrapper<WorkoutDto>.Failed($"An error occurred during the delete request: {ex.Message}");

            return BadRequest(error);
        }
    }
}
