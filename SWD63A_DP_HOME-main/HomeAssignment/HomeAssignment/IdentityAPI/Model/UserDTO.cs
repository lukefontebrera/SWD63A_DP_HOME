using System.ComponentModel.DataAnnotations;

namespace IdentityAPI.Model
{
    public class UserDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
