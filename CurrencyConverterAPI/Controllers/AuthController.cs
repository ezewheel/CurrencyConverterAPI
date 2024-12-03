using Common.DTOs;
using Common.DTOs.UserDTOs;
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

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_authService.Get());
        }

        [HttpPost]
        public IActionResult Login([FromBody] UserForLoginDto userForLogin)
        {
            string? token = _authService.Login(userForLogin);
            if (token is null)
            {
                return Unauthorized(new ResponseDto<string>()
                {
                    Message = "Invalid username or password"
                });
            }

            return Ok(new ResponseDto<string>()
            {
                Message = "Success",
                Data = token
            });
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] UserForRegistrationDto userForRegistration)
        {
            string? token = _authService.Register(userForRegistration);
            if (token is null)
            {
                return BadRequest(new ResponseDto<string>()
                {
                    Message = "User already exists"
                });
            }

            return Ok(new ResponseDto<string>()
            {
                Message = "Success",
                Data = token
            });
        }
    }
}
