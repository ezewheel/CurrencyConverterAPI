using Common.DTOs;
using Common.DTOs.ConversionDTOs;
using Common.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.ConversionService;
using System.Security.Claims;

namespace CurrencyConverterAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ConversionController : ControllerBase
    {
        private readonly IConversionService _conversionService;
        public ConversionController(IConversionService conversionService)
        {
            _conversionService = conversionService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            string username = User.FindFirst(ClaimTypes.Name)?.Value!;
            List<ConversionForViewDto>? conversions = _conversionService.Get(username);
            return Ok(new ResponseDto<List<ConversionForViewDto>>()
            {
                Message = "Success",
                Data = conversions
            });
        }

        [HttpPost]
        public IActionResult Convert([FromBody] ConversionCurrenciesDto conversionCurrencies)
        {
            string username = User.FindFirst(ClaimTypes.Name)?.Value!;
            ConversionResultDto conversionResult = _conversionService.Convert(username, conversionCurrencies);

            if (conversionResult.Status == ConversionResultEnum.Success)
            {
                return Ok(new ResponseDto<decimal?>()
                {
                    Message = "Success",
                    Data = conversionResult.Result
                });
            }

            if (conversionResult.Status == ConversionResultEnum.UserUnsubscribed)
            {
                return Unauthorized(new ResponseDto<decimal>()
                {
                    Message = "User unsubscribed"
                });
            }

            if (conversionResult.Status == ConversionResultEnum.CurrencyNotFound)
            {
                return BadRequest(new ResponseDto<decimal>()
                {
                    Message = "Currency not found"
                });
            }

            if (conversionResult.Status == ConversionResultEnum.UnvalidAmount)
            {
                return BadRequest(new ResponseDto<decimal>()
                {
                    Message = "Invalid amount"
                });
            }

            return Unauthorized(new ResponseDto<decimal>()
            {
                Message = "Conversions limit exceeded"
            });
        }
    }
}