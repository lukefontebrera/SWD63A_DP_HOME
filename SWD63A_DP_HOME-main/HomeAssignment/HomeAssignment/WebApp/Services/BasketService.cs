using SharedModels.Models;
using System.Net.Http.Json;

namespace WebApp.Services
{
    public class BasketService : IBasketService
    {
        private readonly HttpClient _httpClient;

        public BasketService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<BasketItemDTO>> GetItems()
        {
            try
            {
                var basket = await _httpClient.GetFromJsonAsync<IEnumerable<BasketItemDTO>>("gateway/BasketItems");
                return basket;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task AddToBasket(BasketItemDTO basketItem)
        {

            var response = await _httpClient.PostAsJsonAsync("gateway/BasketItems", basketItem);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteItem(string itemId)
        {
            var response = await _httpClient.DeleteAsync($"gateway/BasketItems/{itemId}");
            response.EnsureSuccessStatusCode();
        }
    }
}
