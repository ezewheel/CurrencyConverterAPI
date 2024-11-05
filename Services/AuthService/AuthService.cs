using Common.UserDTOs;
using Data.Entities;
using Data.Repositories.AuthRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _config;
        private readonly IUserRepository _userRepository;

        public AuthService( IConfiguration config, IUserRepository userRepository)
        {
            _config = config;
            _userRepository = userRepository;
        }

        private string GenerateJwtToken(User user)
        {
            var securityPassword = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config["Authentication:SecretForKey"]));

            SigningCredentials signature = new SigningCredentials(securityPassword, SecurityAlgorithms.HmacSha256);

            var claimsForToken = new List<Claim>();
            claimsForToken.Add(new Claim(ClaimTypes.Name, user.Name));

            var jwtSecurityToken = new JwtSecurityToken(
              _config["Authentication:Issuer"],
              _config["Authentication:Audience"],
              claimsForToken,
              DateTime.UtcNow,
              DateTime.UtcNow.AddHours(1),
              signature);

            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }

        public List<User> Get()
        {
            return _userRepository.Get();
        }

        private bool ValidatePassword(User user, string password)
        {
            PasswordHasher<User> passwordHasher = new PasswordHasher<User>();
            return passwordHasher
                .VerifyHashedPassword(user, user.Password, password) == PasswordVerificationResult.Success;
        }

        public string? Login(UserForLoginDto userForLogin)
        {
            User? user = _userRepository.Get(userForLogin.Username);
            if (user is null || !ValidatePassword(user, userForLogin.Password))
            {
                return null;
            }

            return GenerateJwtToken(user);
        }

        private string HashPassword(User user, string password)
        {
            PasswordHasher<User> passwordHasher = new PasswordHasher<User>();
            return passwordHasher
                .HashPassword(user, password);
        }

        public string? Register(UserForRegistrationDto userForRegistration)
        {
            User? user = _userRepository.Get(userForRegistration.Username);
            if (user is not null)
            {
                return null;
            }

            user = new User
            {
                Username = userForRegistration.Username,
                Name = userForRegistration.Username,
                Password = "",
                SubscriptionId = null,
                Subscription = null,
                SubscribedUntil = null,
                ConversionsCount = 0,
                usedTrial = false,
                isDeleted = false
            };

            user.Password = HashPassword(user, userForRegistration.Password);

            _userRepository.Add(user);
            return GenerateJwtToken(user);
        }
    }
}