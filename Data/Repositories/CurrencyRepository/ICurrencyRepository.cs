using Data.Entities;

namespace Data.Repositories.CurrencyRepository
{
    public interface ICurrencyRepository
    {
        List<Currency> Get();
        Currency? Get(int id);
        Currency? Get(string symbol);
        void Add(Currency currency);
        void Update(Currency currency);
        void Delete(Currency currency);
    }
}