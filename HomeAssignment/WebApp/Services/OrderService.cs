using SharedModels.Models;
using System.Net.Http.Json;

namespace WebApp.Services
{
    public class OrderService : IOrderService
    {
        private readonly HttpClient _httpClient;

        public OrderService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task AddOrder(OrderDTO order)
        {
            var response = await _httpClient.PostAsJsonAsync("gateway/Orders", order);
            response.EnsureSuccessStatusCode();
        }

        public async Task<IEnumerable<OrderDTO>> GetItems()
        {
            try
            {
                var order = await _httpClient.GetFromJsonAsync<IEnumerable<OrderDTO>>("gateway/Orders");
                return order;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<OrderDTO> GetItem(string id)
        {
            try
            {
                var order = await _httpClient.GetFromJsonAsync<OrderDTO>($"gateway/Orders/{id}");
                return order;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}