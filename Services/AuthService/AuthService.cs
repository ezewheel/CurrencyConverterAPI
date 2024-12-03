using Common.DTOs.UserDTOs;
using Data.Entities;
using Data.Repositories.AuthRepository;
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
            var securityPassword = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config["Authentication:SecretForKey"]!));

            SigningCredentials signature = new SigningCredentials(securityPassword, SecurityAlgorithms.HmacSha256);

            var claimsForToken = new List<Claim>();
            claimsForToken.Add(new Claim(ClaimTypes.Name, user.Username));
            if (user.Subscription is not null)
            {
                claimsForToken.Add(new Claim("role", user.Subscription.Name));
            }

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

        public string? Login(UserForLoginDto userForLogin)
        {
            User? user = _userRepository.Get(userForLogin.Username);
            if (user is null || user.Password != userForLogin.Password)
            {
                return null;
            }

            return GenerateJwtToken(user);
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
                Password = userForRegistration.Password,
                SubscriptionId = null,
                Subscription = null,
                SubscribedUntil = null,
                ConversionsCount = 0,
                usedTrial = false,
                isDeleted = false
            };

            _userRepository.Add(user);
            return GenerateJwtToken(user);
        }
    }
}