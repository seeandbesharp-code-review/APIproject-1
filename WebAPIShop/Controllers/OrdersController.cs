using DTOs;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Repositories;
using Servers;
using System.Text.Json;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPIShop.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {

        private readonly IOrdersService _ordersService;
        
        public OrdersController(IOrdersService ordersService)
        {
            _ordersService = ordersService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDTO>> Get(int id) 
        {
            OrderDTO order = await _ordersService.GetOrderById(id);
            if(order != null)
            {
                return Ok(order);
            }
            return NoContent();
        }
  
        [HttpPost]
        public async Task<ActionResult<OrderDTO>> Post([FromBody] OrderDTO order)
        {
            OrderDTO createdOrder = await _ordersService.AddOrder(order);
            if(createdOrder != null)
                return CreatedAtAction(nameof(Get), new{id = createdOrder.userId}, createdOrder);
            return BadRequest("order d'ont eccept!!");
        }


       
    }
}
