using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories.ConversionRepository
{
    public class ConversionRepository : IConversionRepository
    {
        private readonly CurrencyConverterContext _context;
        public ConversionRepository(CurrencyConverterContext context)
        {
            _context = context;
        }
        public void Add(Conversion conversion)
        {
            _context.Conversions.Add(conversion);
            _context.SaveChanges();
        }

        public List<Conversion> Get(int id)
        {
            return _context.Conversions
                .Include(c => c.User)
                .Include(c => c.FromCurrency)
                .Include(c => c.ToCurrency)
                .Where(c => c.UserId == id)
                .ToList();
        }
    }
}
