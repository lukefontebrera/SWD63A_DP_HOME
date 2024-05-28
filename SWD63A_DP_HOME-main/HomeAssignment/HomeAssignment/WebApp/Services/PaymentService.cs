using SharedModels.Models;
using System.Net.Http.Json;

namespace WebApp.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly HttpClient _httpClient;

        public PaymentService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task AddPayment(PaymentDTO payment)
        {
            var response = await _httpClient.PostAsJsonAsync("gateway/Payments", payment);
            response.EnsureSuccessStatusCode();
        }

        public async Task<IEnumerable<PaymentDTO>> GetItems()
        {
            try
            {
                var payment = await _httpClient.GetFromJsonAsync<IEnumerable<PaymentDTO>>("gateway/Payments");
                return payment;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
