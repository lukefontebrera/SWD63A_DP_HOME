using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using SharedModels.Models;
using System.Security.Claims;
using WebApp.Services;

namespace WebApp.Pages
{
    public class PaymentBase : ComponentBase
    {
        [Inject]
        public IPaymentService PaymentService { get; set; }

        [Inject]
        public AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        public IEnumerable<PaymentDTO> Payments { get; set; }

        public string UserEmail { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var authenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authenticationState.User;

            if (user.Identity.IsAuthenticated)
            {
                var emailClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
                if (emailClaim != null)
                {
                    UserEmail = emailClaim.Value;
                }
            }

            var items = await PaymentService.GetItems();
            Payments = items.Where(item => item.User == UserEmail);
        }
    }
}
