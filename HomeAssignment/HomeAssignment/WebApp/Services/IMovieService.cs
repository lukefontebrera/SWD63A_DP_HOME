using SharedModels.Models;

namespace WebApp.Services
{
    public interface IBasketService
    {
        Task<IEnumerable<BasketItemDTO>> GetItems();
    }
}
