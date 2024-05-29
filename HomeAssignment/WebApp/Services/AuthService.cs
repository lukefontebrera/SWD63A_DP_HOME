using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Runtime.InteropServices;
using WebApp.Models;

namespace WebApp.Services
{
	public class AuthService : IAuthService
	{
		public readonly HttpClient _httpClient;
		public readonly AuthenticationStateProvider _authenticationStateProvider;
		private readonly ILocalStorageService _localStorageService;

		public AuthService(HttpClient httpClient,
			AuthenticationStateProvider authenticationStateProvider,
			ILocalStorageService localStorageService)
		{
			_httpClient = httpClient;
			_authenticationStateProvider = authenticationStateProvider;
			_localStorageService = localStorageService;
		}

		public async Task<RegisterResult> Register(RegisterModel registerModel)
		{
			var result = await _httpClient.PostAsJsonAsync("gateway/identity/register", registerModel);

			RegisterResult registerResult = new RegisterResult();

			registerResult.Successful = result.IsSuccessStatusCode;

			if (!result.IsSuccessStatusCode)
			{
				registerResult.Errors = new List<string>();
				//TODO add errors
			}

			return registerResult;
		}

		public async Task<LoginResult> Login(LoginModel loginModel)
		{
			var result = await _httpClient.PostAsJsonAsync("gateway/identity/login", loginModel);

			LoginResult loginResult = new LoginResult();

			loginResult.Successful = result.IsSuccessStatusCode;

			if (!result.IsSuccessStatusCode)
			{
				loginResult.Error = "Authentication was not successful";
				return loginResult;
			}

			loginResult.Token = await result.Content.ReadAsStringAsync();

			await _localStorageService.SetItemAsync("authToken", loginResult.Token);

			((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(loginModel.Email);
			_httpClient.DefaultRequestHeaders.Authorization =
				new AuthenticationHeaderValue("Bearer", loginResult.Token);

			return loginResult;
		}

		public async Task Logout()
		{
			await _localStorageService.RemoveItemAsync("authToken");
			((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
			_httpClient.DefaultRequestHeaders.Authorization = null;
		}
	}
}
