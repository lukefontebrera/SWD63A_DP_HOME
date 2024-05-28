using SharedModels.Models;
using System.Net.Http.Json;

namespace WebApp.Services
{
	public class UserService
	{
		private readonly HttpClient _httpClient;

		public UserService(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		public async Task<UserDTO> GetUserByEmailAsync(string email)
		{
			var response = await _httpClient.GetAsync($"gateway/identity/user?email={email}");
			response.EnsureSuccessStatusCode();

			return await response.Content.ReadFromJsonAsync<UserDTO>();
		}
	}
}
