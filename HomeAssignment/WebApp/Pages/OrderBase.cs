using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using SharedModels.Models;
using System.Security.Claims;
using WebApp.Services;

namespace WebApp.Pages
{
    public class OrderBase : ComponentBase
    {
        [Inject]
        public IOrderService OrderService { get; set; }

        [Inject]
        public AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        public IEnumerable<OrderDTO> Orders { get; set; }

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

            var items = await OrderService.GetItems();
            Orders = items.Where(item => item.User == UserEmail);
        }
    }
}
