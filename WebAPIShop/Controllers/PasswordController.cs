using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
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

        [AllowAnonymous] // used during registration flow before user has an account
        [HttpPost]
        public ActionResult<Password> Post([FromBody] Password password)
        {
            Password result = _passwordService.CheckPassword(password.ThePassword);
            return Ok(result);
        }
    }
}
