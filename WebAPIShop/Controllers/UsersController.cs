using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Servers;
using DTOs;

namespace WebAPIShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IUserService _userService;

        public UsersController(IUserService userService, ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [AuthorizeRoles("Admin")] // only admin can look up any user by id
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> Get(int id)
        {
            UserDTO user = await _userService.GetUserById(id);
            if (user != null) return Ok(user);
            return NotFound();
        }

        [AllowAnonymous] // anyone can register
        [HttpPost]
        public async Task<ActionResult<UserDTO>> Post([FromBody] UserWithPasswordDTO user)
        {
            ResultValidUser<UserDTO> createdUser = await _userService.AddUser(user);
            if (createdUser.data != null)
            {
                string token = _userService.GenerateToken(createdUser.data);
                AttachTokenCookie(token);
                return CreatedAtAction(nameof(Get), new { id = createdUser.data.UserId }, createdUser.data);
            }
            if (createdUser.InvalidEmail) return BadRequest("Email is not valid");
            if (createdUser.InvalidPassword) return BadRequest("Password is not strong enough");
            return BadRequest("This email is already in use");
        }

        [AllowAnonymous] // anyone can attempt to log in
        [HttpPost("login")]
        public async Task<ActionResult<object>> Login([FromBody] LoginUserDTO loginUser)
        {
            UserDTO user = await _userService.Login(loginUser.UserEmail, loginUser.UserPassword);
            if (user != null)
            {
                _logger.LogInformation("Login: {Email}", loginUser.UserEmail);
                string token = _userService.GenerateToken(user);
                AttachTokenCookie(token);
                return Ok(new { user, token }); // token exposed for Swagger testing
            }
            return NoContent();
        }

        [Authorize] // must be logged in to log out
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwt");
            return Ok();
        }

        [AuthorizeRoles("Admin")] // only admin can update any user's profile
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] UserWithPasswordDTO user)
        {
            ResultValidUser<bool> result = await _userService.UpdateUser(id, user);
            if (result.data) return Ok();
            if (result.InvalidEmail) return BadRequest("Email is not valid");
            if (result.InvalidPassword) return BadRequest("Password is not strong enough");
            return BadRequest("This email is already in use");
        }

        private void AttachTokenCookie(string token)
        {
            bool isProduction = HttpContext.RequestServices
                .GetRequiredService<IWebHostEnvironment>().IsProduction();

            Response.Cookies.Append("jwt", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = isProduction,
                SameSite = isProduction ? SameSiteMode.Strict : SameSiteMode.Lax,
                Expires = DateTimeOffset.UtcNow.AddHours(2)
            });
        }
    }
}
