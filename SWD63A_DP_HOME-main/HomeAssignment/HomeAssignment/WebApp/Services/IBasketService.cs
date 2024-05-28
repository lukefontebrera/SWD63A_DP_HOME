using SharedModels.Models;

namespace WebApp.Services
{
    public interface IBasketService
    {
        Task<IEnumerable<BasketItemDTO>> GetItems();

        Task AddToBasket(BasketItemDTO basketItem);

        Task DeleteItem(string itemId);
    }
}
