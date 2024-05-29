using Microsoft.AspNetCore.Components;
using SharedModels.Models;
using WebApp.Services;

namespace WebApp.Pages
{
    public class PaymentBase : ComponentBase
    {
        [Inject]
        public IPaymentService PaymentService { get; set; }

        public IEnumerable<PaymentDTO> Payments { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Payments = await PaymentService.GetItems();
        }
    }
}
