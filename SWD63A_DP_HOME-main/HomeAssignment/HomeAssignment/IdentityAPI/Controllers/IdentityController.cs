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
		private readonly ILogger<IdentityController> _logger;

		public IdentityController(IdentityService context, IJwtBuilder jwtBuilder,
			IEncryptor encryptor, ILogger<IdentityController> logger)
		{
			_context = context;
			_jwtBuilder = jwtBuilder;
			_encryptor = encryptor;
			_logger = logger;
		}

		[HttpPost("register")]
		public async Task<ActionResult> Register([FromBody] UserDTO userDTO)
		{
			try
			{
				var u = await _context.GetByEmailAsync(userDTO.Email);

				if (u != null)
				{
					return BadRequest("User already exists");
				}

				var user = new User();
				user.SetPassword(userDTO.Password, _encryptor);
				user.Email = userDTO.Email;

				await _context.CreateAsync(user);

				return Ok();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error occurred during registration.");
				return StatusCode(500, "Internal server error");
			}
		}

		[HttpPost("login")]
		public async Task<ActionResult> Login([FromBody] UserDTO userDTO)
		{
			try
			{
				_logger.LogInformation($"Login attempt for email: {userDTO.Email}");

				var u = await _context.GetByEmailAsync(userDTO.Email.ToLowerInvariant());

				if (u == null)
				{
					_logger.LogWarning($"User not found: {userDTO.Email}");
					return NotFound("User not found");
				}

				var isValid = u.ValidatePassword(userDTO.Password, _encryptor);

				if (isValid)
				{
					var token = _jwtBuilder.GetToken(u.Id);
					_logger.LogInformation($"Login successful for email: {userDTO.Email}");
					return Ok(token);
				}
				else
				{
					_logger.LogWarning($"Invalid password for email: {userDTO.Email}");
					return BadRequest("Could not authenticate user");
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error occurred during login.");
				return StatusCode(500, "Internal server error");
			}
		}

		[HttpGet("validate")]
		public async Task<ActionResult> Validate([FromQuery(Name = "email")] string email,
			[FromQuery(Name = "token")] string token)
		{
			try
			{
				var u = await _context.GetByEmailAsync(email.ToLowerInvariant());

				if (u == null)
				{
					return NotFound("User not found");
				}

				var userId = _jwtBuilder.ValidateToken(token);

				if (userId != u.Id)
				{
					return BadRequest("Invalid Token");
				}
				else
				{
					return Ok(userId);
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error occurred during token validation.");
				return StatusCode(500, "Internal server error");
			}
		}

		[HttpGet("user")]
		public async Task<ActionResult<User>> GetUserByEmail([FromQuery] string email)
		{
			var user = await _context.GetByEmailAsync(email);
			if (user == null)
			{
				return NotFound();
			}
			return user;
		}
	}
}
