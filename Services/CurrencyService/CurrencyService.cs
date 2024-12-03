using Common.DTOs.CurrencyDTOs;
using Common.Enums;
using Data.Entities;
using Data.Repositories.CurrencyRepository;

namespace Services.CurrenciesService
{
    public class CurrencyService : ICurrencyService
    {
        private readonly ICurrencyRepository _currencyRepository;
        public CurrencyService(ICurrencyRepository currencyRepository)
        {
            _currencyRepository = currencyRepository;
        }

        public List<CurrencyForConversionViewDto> GetForConversion()
        {
            return _currencyRepository
                .Get()
                .Select(c => new CurrencyForConversionViewDto()
                {
                    Name = c.Name,
                    Symbol = c.Symbol,
                    CountryCode = c.CountryCode
                })
                .ToList();
        }

        public List<CurrencyForViewDto> Get()
        {
            return _currencyRepository.Get()
                .Where(c => c.IsDeleted == false)
                .Select(c => new CurrencyForViewDto()
                {
                    Id = c.Id,
                    Name = c.Name,
                    Symbol = c.Symbol,
                    ConversionRate = c.ConversionRate,
                    CountryCode = c.CountryCode
                })
                .ToList();
        }

        public CurrencyForViewDto? Get(int id)
        {
            Currency? currency = _currencyRepository.Get(id);
            if (currency is null)
            {
                return null;
            }

            return new CurrencyForViewDto()
            {
                Id = currency.Id,
                Name = currency.Name,
                Symbol = currency.Symbol,
                ConversionRate = currency.ConversionRate,
                CountryCode = currency.CountryCode
            };
        }

        public CurrencyForViewDto? Get(string symbol)
        {
            Currency? currency = _currencyRepository.Get(symbol);
            if (currency is null)
            {
                return null;
            }

            return new CurrencyForViewDto()
            {
                Id = currency.Id,
                Name = currency.Name,
                Symbol = currency.Symbol,
                ConversionRate = currency.ConversionRate,
                CountryCode = currency.CountryCode
            };
        }

        public CurrencyResultEnum Add(CurrencyForAddDto currencyForAdd)
        {
            if (currencyForAdd.ConversionRate <= 0)
            {
                return CurrencyResultEnum.InvalidConversionRate;
            }

            Currency? currency = _currencyRepository.Get(currencyForAdd.Symbol);
            if (currency is not null)
            {
                return CurrencyResultEnum.AlreadyExists;
            }

            _currencyRepository.Add(new Currency()
            {
                Name = currencyForAdd.Name,
                Symbol = currencyForAdd.Symbol,
                ConversionRate = currencyForAdd.ConversionRate,
                CountryCode = currencyForAdd.CountryCode
            });

            return CurrencyResultEnum.Success;

        }

        public CurrencyResultEnum Update(CurrencyForUpdateDto currencyForUpdate)
        {
            if (currencyForUpdate.ConversionRate <= 0)
            {
                return CurrencyResultEnum.InvalidConversionRate;
            }

            Currency? currency = _currencyRepository.Get(currencyForUpdate.Symbol);
            if (currency is not null)
            {
                return CurrencyResultEnum.AlreadyExists;
            }

            currency = _currencyRepository.Get(currencyForUpdate.Id);
            if (currency is null)
            {
                return CurrencyResultEnum.DoesNotExist;
            }

            currency.Name = currencyForUpdate.Name;
            currency.Symbol = currencyForUpdate.Symbol;
            currency.ConversionRate = currencyForUpdate.ConversionRate;
            currency.CountryCode = currencyForUpdate.CountryCode;

            _currencyRepository.Update(currency);
            return CurrencyResultEnum.Success;
        }

        public bool Delete(int id)
        {
            Currency? currency = _currencyRepository.Get(id);
            if (currency is null)
            {
                return false;
            }

            _currencyRepository.Delete(currency);
            return true;
        }
    }
}
