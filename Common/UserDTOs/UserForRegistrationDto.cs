using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Common.UserDTOs
{
    public class UserForRegistrationDto
    {
        public required string Username { get; set; }
        public required string Name { get; set; }
        public required string Password { get; set; }
    }
}
