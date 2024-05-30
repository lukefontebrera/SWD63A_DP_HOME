using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using SharedModels.Models;
using System.Security.Claims;
using WebApp.Services;

namespace WebApp.Pages
{
    public class BasketItemsBase : ComponentBase
    {
        [Inject]
        public IBasketService BasketService { get; set; }

        [Inject]
        public AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        public IEnumerable<BasketItemDTO> BasketItems { get; set; }
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

            await LoadBasketItems();
        }

        protected async Task LoadBasketItems()
        {
            var items = await BasketService.GetItems();
            BasketItems = items.Where(item => item.User == UserEmail);
        }

        protected async Task DeleteItem(string itemId)
        {
            await BasketService.DeleteItem(itemId);
            await LoadBasketItems();
        }
    }
}
