using AutoMapper;
using Bdt.Api.Application.Services.Interfaces;
using Bdt.Api.Domain.Entities;
using Bdt.Api.Infrastructure.Exceptions.Api;
using Bdt.Shared.Dtos.Planner;
using Bdt.Shared.Models.App;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Bdt.Api.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize]
public class PlannerController : BaseController<Guid, PlannerEntity, PlannerDto, UpdatePlannerDto, CreatePlannerDto, DeletePlannerDto>
{
    protected readonly IPlannerService _plannerService;
    public PlannerController(IMapper mapper,
        IGenericService<Guid, PlannerEntity, PlannerDto, UpdatePlannerDto, CreatePlannerDto, DeletePlannerDto> service,
        IPlannerService plannerService)
        : base(mapper, service)
    {
        _plannerService = plannerService;
    }

    [HttpGet("GetUserPlans")]
    public async Task<ActionResult<ApiWrapper<IEnumerable<PlannerDto>>>> GetAllByUserId()
    {
        try
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return BadRequest(ApiWrapper<IEnumerable<PlannerDto>>.Failed("User identifier is missing."));

            var plans = await _plannerService.GetAllByUserId(userId);

            var userPlansDto = _mapper.Map<IEnumerable<PlannerDto>>(plans);

            return Ok(ApiWrapper<IEnumerable<PlannerDto>>.Success(userPlansDto));
        }
        catch (UserFriendlyException ex)
        {
            var error = ApiWrapper<IEnumerable<PlannerDto>>.Failed(ex.Message);

            return BadRequest(error);
        }
        catch (Exception ex)
        {
            var error = ApiWrapper<IEnumerable<PlannerDto>>.Failed($"An error occurred during the create request: {ex.Message}");

            return BadRequest(error);
        }
    }
}
