using Data.Entities;

namespace Data.Repositories.ConversionRepository
{
    public interface IConversionRepository
    {
        void Add(Conversion conversion);
        List<Conversion> Get(int id);
    }
}
