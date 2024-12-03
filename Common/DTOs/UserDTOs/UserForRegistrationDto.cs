using System.ComponentModel.DataAnnotations;

namespace Common.DTOs.UserDTOs
{
    public class UserForRegistrationDto
    {
        [MinLength(1)]
        public required string Username { get; set; }
        [MinLength(1)]
        public required string Name { get; set; }
        [MinLength(1)]
        public required string Password { get; set; }
    }
}
