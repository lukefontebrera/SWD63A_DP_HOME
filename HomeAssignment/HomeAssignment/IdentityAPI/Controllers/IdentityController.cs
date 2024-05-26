using Microsoft.AspNetCore.Mvc;
using IdentityAPI.Services;
using Middleware;
using IdentityAPI.Model;
using System.ComponentModel.DataAnnotations;

namespace IdentityAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class IdentityController : ControllerBase
	{
		private readonly IdentityService _context;
		private readonly IJwtBuilder _jwtBuilder;
		private readonly IEncryptor _encryptor;

		public IdentityController(IdentityService context, IJwtBuilder jwtBuilder,
			IEncryptor encryptor)
		{
			_context = context;
			_jwtBuilder = jwtBuilder;
			_encryptor = encryptor;
		}

		[HttpPost("register")]
		public async Task<ActionResult> Register([FromBody] UserDTO userDTO)
		{
			var u = await _context.GetByEmailAsync(userDTO.Email);

			if (u != null) {
				return BadRequest("User already exists");
			}

			var user = new User();

			user.SetPassword(userDTO.Password, _encryptor);
			user.Email = userDTO.Email;

			await _context.CreateAsync(user);

			return Ok();
		}

		[HttpPost("login")]
		public async Task<ActionResult> Login([FromBody] UserDTO userDTO)
		{
			var u = await _context.GetByEmailAsync(userDTO.Email);

			if (u == null)
			{
				return NotFound("User not found");
			}

			var isValid = u.ValidatePassword(userDTO.Password, _encryptor);

			if (isValid) {
				var token = _jwtBuilder.GetToken(u.Id);
				return Ok(token);
			}
			else
			{
				return BadRequest("Could not authenticate user");
			}

		}

		[HttpGet("validate")]
		public async Task<ActionResult> Validate([FromQuery(Name = "email")] string email,
			[FromQuery(Name = "token")] string token)
		{
			var u = await _context.GetByEmailAsync(email);

			if(u == null)
			{
				return NotFound("User not found");
			}

			var userId = _jwtBuilder.ValidateToken(token);

			if(userId != u.Id)
			{
				return BadRequest("Invalid Token");
			}
			else
			{
				return Ok(userId);
			}
		}

	}
}
