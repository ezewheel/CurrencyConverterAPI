using Common.UserDTOs;
using Data.Entities;

namespace Services.AuthService
{
    public interface IAuthService
    {
        List<User> Get();
        string? Login(UserForLoginDto userForLogin);
        string? Register(UserForRegistrationDto userForRegistration);
    }
}
