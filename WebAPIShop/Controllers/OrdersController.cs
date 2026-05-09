using DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Servers;

namespace WebAPIShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // baseline: all endpoints require login
    public class OrdersController : ControllerBase
    {
        private readonly IOrdersService _ordersService;

        public OrdersController(IOrdersService ordersService)
        {
            _ordersService = ordersService;
        }

        [AuthorizeRoles("Admin")] // only admin can look up any order by id
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDTO>> Get(int id)
        {
            OrderDTO order = await _ordersService.GetOrderById(id);
            if (order != null) return Ok(order);
            return NoContent();
        }

        [HttpPost] // any logged-in user can place an order
        public async Task<ActionResult<OrderDTO>> Post([FromBody] OrderDTO order)
        {
            OrderDTO createdOrder = await _ordersService.AddOrder(order);
            if (createdOrder != null)
                return CreatedAtAction(nameof(Get), new { id = createdOrder.userId }, createdOrder);
            return BadRequest("Order was not accepted.");
        }
    }
}
