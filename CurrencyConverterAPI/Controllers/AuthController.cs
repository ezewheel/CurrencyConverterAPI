using Common.UserDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.AuthService;

namespace CurrencyConverterAPI.Controllers
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

        [HttpPost]
        public IActionResult Get()
        {
            return Ok(_authService.Get());
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserForLoginDto userForLogin)
        {
            string? token = _authService.Login(userForLogin);
            if (token is null)
            {
                return Unauthorized();
            }

            return Ok(token);
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] UserForRegistrationDto userForRegistration)
        {
            string? token = _authService.Register(userForRegistration);
            if (token is null)
            {
                return BadRequest();
            }

            return Ok(token);
        }
    }
}
