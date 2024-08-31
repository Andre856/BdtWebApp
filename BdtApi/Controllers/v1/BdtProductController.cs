using AutoMapper;
using BdtApi.Application.Services.BdtProduct;
using BdtShared.Dtos.BdtProduct;
using BdtShared.Models.App;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BdtApi.Controllers.v1;

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
        catch (Exception ex) when (ex is AutoMapperConfigurationException || ex is AutoMapperMappingException)
        {
            var error = ApiWrapper<IEnumerable<BdtProductDto>>.Failed($"Automapper exception occurred: {ex.Message}");

            return BadRequest(error);
        }
        catch (Exception ex)
        {
            var error = ApiWrapper<IEnumerable<BdtProductDto>>.Failed($"An error occurred when trying to retrieve from database: {ex.Message}");

            return BadRequest(error);
        }
    }
}
