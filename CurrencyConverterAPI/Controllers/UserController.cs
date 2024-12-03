using Common.DTOs;
using Common.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.UserService;
using System.Security.Claims;

namespace CurrencyConverterAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            string username = User.FindFirst(ClaimTypes.Name)?.Value!;
            List<string> subscriptionTypes = _userService.Get(username);
            return Ok(new ResponseDto<List<string>>()
            {
                Message = "Success",
                Data = subscriptionTypes
            });
        }

        [HttpPut("{subscriptionType}")]
        public IActionResult Subscribe([FromRoute] SubscriptionTypeEnum subscriptionType)
        {
            string username = User.FindFirst(ClaimTypes.Name)?.Value!;
            SubscriptionResultEnum subscriptionResult = _userService.Subscribe(username, subscriptionType);

            if (subscriptionResult == SubscriptionResultEnum.AlreadyUsedTrial)
            {
                return BadRequest(new ResponseDto<string>()
                {
                    Message = "Trial already used"
                });
            }

            if (subscriptionResult == SubscriptionResultEnum.AlreadySubscribed)
            {
                return BadRequest(new ResponseDto<string>()
                {
                    Message = "User already subscribed"
                });
            }

            return Ok(new ResponseDto<string>()
            {
                Message = "Success"
            });
        }
    }
}
