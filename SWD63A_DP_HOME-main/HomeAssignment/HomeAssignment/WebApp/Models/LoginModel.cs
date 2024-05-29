using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class LoginModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }
	}
}
