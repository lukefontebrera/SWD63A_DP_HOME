using System.ComponentModel.DataAnnotations;

namespace SharedModels.Models
{
	public class UserDTO
	{
		public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Password { get; set; }
	}
}
