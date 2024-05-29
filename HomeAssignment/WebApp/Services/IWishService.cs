using SharedModels.Models;

namespace WebApp.Services
{
    public interface IWishService
    {
        Task<IEnumerable<WishDTO>> GetItems();

        Task AddToBasket(WishDTO wishItem);

        Task DeleteItem(string itemId);
    }
}
