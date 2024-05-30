using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using SharedModels.Models;
using WebApp.Services;
using System.Security.Claims;

namespace WebApp.Pages
{
    public class WishItemsBase : ComponentBase
    {
        [Inject]
        public IWishService WishService { get; set; }

        public IEnumerable<WishDTO> WishedItems { get; set; }

        [Inject]
        public AuthenticationStateProvider AuthenticationStateProvider { get; set; }

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

            await LoadWishedItems();
        }

        protected async Task LoadWishedItems()
        {
            var items = await WishService.GetItems();
            WishedItems = items.Where(item => item.User == UserEmail);
        }

        protected async Task DeleteItem(string itemId)
        {
            await WishService.DeleteItem(itemId);
            await LoadWishedItems();
        }
    }
}
