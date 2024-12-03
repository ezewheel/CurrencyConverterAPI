using Data.Entities;

namespace Data.Repositories.CurrencyRepository
{
    public class CurrencyRepository : ICurrencyRepository
    {
        private readonly CurrencyConverterContext _context;

        public CurrencyRepository(CurrencyConverterContext context)
        {
            _context = context;
        }

        public List<Currency> Get()
        {
            return _context.Currencies.Where(c => !c.IsDeleted).ToList();
        }

        public Currency? Get(int id)
        {
            return _context.Currencies.FirstOrDefault(c => c.Id == id && !c.IsDeleted);
        }

        public Currency? Get(string symbol)
        {
            return _context.Currencies.FirstOrDefault(c => c.Symbol == symbol && !c.IsDeleted);
        }

        public void Add(Currency currency)
        {
            _context.Currencies.Add(currency);
            _context.SaveChanges();
        }

        public void Update(Currency currency)
        {
            _context.Currencies.Update(currency);
            _context.SaveChanges();
        }

        public void Delete(Currency currency)
        {
            currency.IsDeleted = true;
            _context.Currencies.Update(currency);
            _context.SaveChanges();
        }
    }
}
