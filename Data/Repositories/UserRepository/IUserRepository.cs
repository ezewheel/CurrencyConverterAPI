using Data.Entities;

namespace Data.Repositories.AuthRepository
{
    public interface IUserRepository
    {
        List<User> Get();
        User? Get(string username);
        void Add(User user);
        void Update(User user);
        void Delete(User user);
    }
}
