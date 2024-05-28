using Microsoft.AspNetCore.Components;
using SharedModels.Models;
using WebApp.Services;

namespace WebApp.Pages
{
	public class BasketItemsBase: ComponentBase
	{
		[Inject]
		public IBasketService BasketService { get; set; }

		public IEnumerable<BasketItemDTO> BasketItems { get; set; }

		protected override async Task OnInitializedAsync()
		{
			BasketItems = await BasketService.GetItems();
		}

        protected async Task DeleteItem(string itemId)
        {
            await BasketService.DeleteItem(itemId);

            BasketItems = await BasketService.GetItems();
        }
    }
}
