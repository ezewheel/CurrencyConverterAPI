using Common.DTOs.CurrencyDTOs;
using Common.Enums;

namespace Services.CurrenciesService
{
    public interface ICurrencyService
    {
        List<CurrencyForConversionViewDto> GetForConversion();
        List<CurrencyForViewDto> Get();
        CurrencyForViewDto? Get(int id);
        CurrencyForViewDto? Get(string symbol);
        CurrencyResultEnum Add(CurrencyForAddDto currency);
        CurrencyResultEnum Update(CurrencyForUpdateDto currency);
        bool Delete(int id);
    }
}