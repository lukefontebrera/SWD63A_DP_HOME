using SharedModels.Models;

namespace WebApp.Services
{
    public interface IPaymentService
    {
        Task<IEnumerable<PaymentDTO>> GetItems();

        Task AddPayment(PaymentDTO payment);
    }
}
