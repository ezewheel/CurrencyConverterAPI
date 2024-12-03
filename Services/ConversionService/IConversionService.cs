using Common.DTOs.ConversionDTOs;

namespace Services.ConversionService
{
    public interface IConversionService
    {
        List<ConversionForViewDto> Get(string username);
        ConversionResultDto Convert(string username, ConversionCurrenciesDto conversionCurrencies);
    }
}
