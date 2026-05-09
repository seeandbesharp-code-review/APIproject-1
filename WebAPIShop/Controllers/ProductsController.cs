using DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Servers;

namespace WebAPIShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IPrudectsService _prudectsService;

        public ProductsController(IPrudectsService prudectsService)
        {
            _prudectsService = prudectsService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<List<ProductDTO>>> Get(
            string? description, int? minPrice, int? maxPrice,
            [FromQuery] int[]? categoriesId, int? limit, string? orderby, int offset = 1)
        {
            PageResponseDTO<ProductDTO> metaData = await _prudectsService.GetProducts(
                description, minPrice, maxPrice, categoriesId, limit, orderby, offset);
            if (metaData != null) return Ok(metaData);
            return NoContent();
        }
    }
}
