using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories.AuthRepository
{
    public class UserRepository : IUserRepository
    {
        private readonly CurrencyConverterContext _context;

        public UserRepository(CurrencyConverterContext context)
        {
            _context = context;
        }

        public List<User> Get()
        {
            return _context.Users.Include(u => u.Subscription).ToList();
        }

        public User? Get(string username)
        {
            return _context.Users.Include(u => u.Subscription).FirstOrDefault(u => u.Username == username);
        }

        public void Add(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void Update(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public void Delete(User user)
        {
            user.isDeleted = true;
            _context.Users.Update(user);
            _context.SaveChanges();
        }
    }
}
