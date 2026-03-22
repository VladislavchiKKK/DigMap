using DigMap.Application.DTOs.Auth;
using DigMap.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace DigMap.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterDto dto)
        {
            var result = await _authService.RegisterAsync(dto);

            if (result.IsSuccess)
            {
                return Ok(new { token = result.Token });
            }

            return BadRequest(result.Errors);
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginDto dto)
        {
            var result = await _authService.LoginAsync(dto);

            if (result.IsSuccess)
            {
                return Ok(new { token = result.Token });
            }

            return Unauthorized(result.Errors);
        }
    }
}