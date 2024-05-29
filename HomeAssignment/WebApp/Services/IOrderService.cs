using SharedModels.Models;

namespace WebApp.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDTO>> GetItems();

        Task<OrderDTO> GetItem(string id);

        Task AddOrder(OrderDTO payment);
    }
}
