using AutoMapper;
using Bdt.Api.Application.Services.Interfaces;
using Bdt.Api.Infrastructure.Exceptions.Api;
using Bdt.Shared.Dtos.BdtProduct;
using Bdt.Shared.Models.App;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bdt.Api.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class BdtProductController : ControllerBase
{
    private readonly IBdtProductService _productService;
    public BdtProductController(IMapper mapper, IBdtProductService productService)
    {
        _productService = productService;
    }

    [AllowAnonymous]
    [HttpGet("GetAll")]
    public async Task<ActionResult<ApiWrapper<IEnumerable<BdtProductDto>>>> GetAll()
    {
        try
        {
            var dtos = await _productService.GetAllAsync();

            return Ok(ApiWrapper<IEnumerable<BdtProductDto>>.Success(dtos));
        }
        catch (UserFriendlyException ex)
        {
            var error = ApiWrapper<IEnumerable<BdtProductDto>>.Failed(ex.Message);

            return BadRequest(error);
        }
        catch (Exception ex)
        {
            var error = ApiWrapper<IEnumerable<BdtProductDto>>.Failed($"An error occurred when trying to retrieve from database: {ex.Message}");

            return BadRequest(error);
        }
    }
}
