using AutoMapper;
using BdtApi.Application.Services.Generic;
using BdtApi.Application.Services.Planner;
using BdtApi.Domain.Entities;
using BdtShared.Dtos.Planner;
using BdtShared.Models.App;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BdtApi.Controllers.v1;

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
        catch (Exception ex) when (ex is AutoMapperConfigurationException || ex is AutoMapperMappingException)
        {
            var error = ApiWrapper<IEnumerable<PlannerDto>>.Failed($"Automapper exception occurred: {ex.Message}");

            return BadRequest(error);
        }
        catch (Exception ex)
        {
            var error = ApiWrapper<IEnumerable<PlannerDto>>.Failed($"An error occurred during the create request: {ex.Message}");

            return BadRequest(error);
        }
    }
}
