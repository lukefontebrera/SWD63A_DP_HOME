using SharedModels.Models;
using System.Net.Http.Json;

namespace WebApp.Services
{
    public class WishService : IWishService
    {
        private readonly HttpClient _httpClient;

        public WishService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<WishDTO>> GetItems()
        {
            try
            {
                var wishedItems = await _httpClient.GetFromJsonAsync<IEnumerable<WishDTO>>("gateway/WishedMovies");
                return wishedItems;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task AddToBasket(WishDTO wishedItem)
        {

            var response = await _httpClient.PostAsJsonAsync("gateway/WishedMovies", wishedItem);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteItem(string itemId)
        {
            var response = await _httpClient.DeleteAsync($"gateway/WishedMovies/{itemId}");
            response.EnsureSuccessStatusCode();
        }
    }
}
