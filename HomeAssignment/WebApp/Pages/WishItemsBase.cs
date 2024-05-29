using Microsoft.AspNetCore.Components;
using SharedModels.Models;
using WebApp.Services;

namespace WebApp.Pages
{
	public class WishItemsBase: ComponentBase
	{
		[Inject]
		public IWishService WishService { get; set; }

		public IEnumerable<WishDTO> WishedItems { get; set; }

		protected override async Task OnInitializedAsync()
		{
            WishedItems = await WishService.GetItems();
		}

        protected async Task DeleteItem(string itemId)
        {
            await WishService.DeleteItem(itemId);

            WishedItems = await WishService.GetItems();
        }
    }
}
