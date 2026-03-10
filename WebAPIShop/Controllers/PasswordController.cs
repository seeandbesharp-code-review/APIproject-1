using Entities;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Servers;
using DTOs;

namespace WebAPIShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PasswordController : ControllerBase
    {
        private readonly IPasswordService _passwordService;

        public PasswordController(IPasswordService passwordService)
        {
            _passwordService = passwordService;
        }

        [HttpPost]
        public ActionResult<Password> Post([FromBody] Password password)
        {
            Password result = _passwordService.CheckPassword(password.ThePassword);
            return Ok(result);
        }
    }
}
