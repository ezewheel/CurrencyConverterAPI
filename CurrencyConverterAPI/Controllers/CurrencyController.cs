using Common.DTOs;
using Common.DTOs.CurrencyDTOs;
using Common.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.CurrenciesService;

namespace CurrencyConverterAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        private readonly ICurrencyService _currenciesService;

        public CurrencyController(ICurrencyService currenciesService)
        {
            _currenciesService = currenciesService;
        }

        [Authorize]
        [HttpGet("conversions")]
        public IActionResult GetForConversion()
        {
            return Ok(new ResponseDto<List<CurrencyForConversionViewDto>>()
            {
                Message = "Success",
                Data = _currenciesService.GetForConversion()
            });
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new ResponseDto<List<CurrencyForViewDto>>()
            {
                Message = "Success",
                Data = _currenciesService.Get()
            });
        }

        [HttpGet("{symbol}")] 
        public IActionResult Get(string symbol)
        {
            CurrencyForViewDto? currency = _currenciesService.Get(symbol);
            if (currency is null)
            {
                return NotFound(new ResponseDto<CurrencyForViewDto>()
                {
                    Message = "Currency not found"
                });
            }
            return Ok(new ResponseDto<CurrencyForViewDto>()
            {
                Message = "Success",
                Data = currency
            });
        }

        [HttpPost]
        public IActionResult Add([FromBody] CurrencyForAddDto currencyForAddDto)
        {
            CurrencyResultEnum currencyResult = _currenciesService.Add(currencyForAddDto);
            if (currencyResult == CurrencyResultEnum.AlreadyExists)
            {
                return BadRequest(new ResponseDto<int>()
                {
                    Message = "Currency already exists"
                });
            }

            if (currencyResult == CurrencyResultEnum.InvalidConversionRate)
            {
                return BadRequest(new ResponseDto<int>()
                {
                    Message = "Invalid conversion rate"
                });
            }

            return Ok(new ResponseDto<int>()
            {
                Message = "Success"
            });
        }

        [HttpPut]
        public IActionResult Update([FromBody] CurrencyForUpdateDto currencyForUpdateDto)
        {
            CurrencyResultEnum currencyResult = _currenciesService.Update(currencyForUpdateDto);
            if (currencyResult == CurrencyResultEnum.AlreadyExists)
            {
                return BadRequest(new ResponseDto<string>()
                {
                    Message = "Currency already exists"
                });
            }

            if (currencyResult == CurrencyResultEnum.InvalidConversionRate)
            {
                return BadRequest(new ResponseDto<string>()
                {
                    Message = "Invalid conversion rate"
                });
            }

            if (currencyResult == CurrencyResultEnum.DoesNotExist)
            {
                return BadRequest(new ResponseDto<string>()
                {
                    Message = "Currency does not exist"
                });
            }

            return Ok(new ResponseDto<string>()
            {
                Message = "Success"
            });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (!_currenciesService.Delete(id))
            {
                return NotFound(new ResponseDto<string>()
                {
                    Message = "Currency not found"
                });
            }
            return Ok(new ResponseDto<string>
            {
                Message = "Success"
            });
        }
    }
}
