using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Servers;
using Entities;
using Repositories;
using DTOs;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPIShop.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {

        private readonly ICategoryService _categoryService;
        
        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<List<CategoryDTO>>> Get() 
        {
            List<CategoryDTO> categories = await _categoryService.GetCategories();
            if(categories != null)
            {
                return Ok(categories);
            }
            return NoContent();
        }
    }
}
