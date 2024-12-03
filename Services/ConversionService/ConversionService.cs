using Common.DTOs.ConversionDTOs;
using Common.Enums;
using Data.Entities;
using Data.Repositories.AuthRepository;
using Data.Repositories.ConversionRepository;
using Data.Repositories.CurrencyRepository;

namespace Services.ConversionService
{
    public class ConversionService : IConversionService
    {
        private readonly IConversionRepository _conversionRepository;
        private readonly ICurrencyRepository _currencyRepository;
        private readonly IUserRepository _userRepository;
        public ConversionService(IConversionRepository conversionRepository,
            ICurrencyRepository currencyRepository,
            IUserRepository userRepository)
        {
            _conversionRepository = conversionRepository;
            _currencyRepository = currencyRepository;
            _userRepository = userRepository;
        }

        public List<ConversionForViewDto> Get(string username)
        {
            User user = _userRepository.Get(username)!;

            return _conversionRepository
                .Get(user.Id)
                .Select(c => new ConversionForViewDto()
                {
                    FromCurrency = c.FromCurrency.Symbol,
                    ToCurrency = c.ToCurrency.Symbol,
                    Amount = c.Amount,
                    Result = c.Result,
                    Date = c.Date.ToString("dd/MM/yyyy HH:mm")
                })
                .ToList();
        }

        private ConversionResultEnum ValidateUser(User? user)
        {
            if (user!.Subscription is null)
            {
                return ConversionResultEnum.UserUnsubscribed;
            }

            if (user.SubscribedUntil < DateTime.UtcNow)
            {
                user.SubscriptionId = null;
                user.SubscribedUntil = null;
                _userRepository.Update(user);
                return ConversionResultEnum.UserUnsubscribed;
            }

            //int Conversions = _conversionRepository
            //    .Get(user.Id)
            //    .Where(c => c.Date > user.SubscribedUntil!
            //    .Value
            //    .AddMonths(-1))
            //    .Count();

            if (user.Subscription.ConversionsLimit is not null &&
                user.ConversionsCount >= user.Subscription.ConversionsLimit)
            {
                return ConversionResultEnum.LimitExceeded;
            }

            return ConversionResultEnum.Success;

        }

        private ConversionResultEnum ValidateCurrencies(Currency? from, Currency? to, decimal amount)
        {
            if (from is null || to is null)
            {
                return ConversionResultEnum.CurrencyNotFound;
            }

            if (amount <= 0)
            {
                return ConversionResultEnum.UnvalidAmount;
            }

            return ConversionResultEnum.Success;
        }

        private void UpdateConversionsCount(User user)
        {
            user.ConversionsCount++;
            _userRepository.Update(user);
        }

        public ConversionResultDto Convert(string username, ConversionCurrenciesDto conversionCurrencies)
        {
            User? user = _userRepository.Get(username);
            
            ConversionResultEnum UserValidation = ValidateUser(user);
            if (UserValidation is not ConversionResultEnum.Success)
            {
                return new ConversionResultDto()
                {
                    Status = UserValidation
                };
            }

            Currency? From = _currencyRepository.Get(conversionCurrencies.From);
            Currency? To = _currencyRepository.Get(conversionCurrencies.To);
            
            ConversionResultEnum CurrenciesValidation = ValidateCurrencies(From, To, conversionCurrencies.Amount);
            if (CurrenciesValidation is not ConversionResultEnum.Success)
            {
                return new ConversionResultDto()
                {
                    Status = CurrenciesValidation
                };
            }

            decimal result = From!.ConversionRate / To!.ConversionRate * conversionCurrencies.Amount;

            _conversionRepository.Add(new Conversion
            {
                FromCurrencyId = From.Id,
                ToCurrencyId = To.Id,
                Amount = conversionCurrencies.Amount,
                Result = result,
                Date = DateTime.UtcNow,
                UserId = user!.Id
            });

            UpdateConversionsCount(user);
            return new ConversionResultDto()
            {
                Status = ConversionResultEnum.Success,
                Result = result
            };
        }
    }
}
