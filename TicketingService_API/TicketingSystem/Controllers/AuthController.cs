using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using TicketingSystem.Exceptions;
using TicketingSystem.Models;
using TicketingSystem.Services.Interface;

namespace TicketingSystem.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterRequest request)
        {
            try
            {
                var result = await _userService.RegisterAsync(request);
                return Ok(result);
            }
            catch (AuthException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginRequest request)
        {
            try
            {
                var authToken = await _userService.LoginAsync(request);
                var response = new
                {
                    Token = authToken,
                    Message = "Login successful."
                };
                return Ok(response);
            }
            catch (AuthException ex)
            {
                return Unauthorized(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }
    }
}
