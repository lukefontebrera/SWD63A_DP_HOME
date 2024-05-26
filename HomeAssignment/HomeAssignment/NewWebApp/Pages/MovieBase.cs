using Microsoft.AspNetCore.Components;
using SharedModels.Models;
using NewWebApp.Services;

namespace NewWebApp.Pages
{
	public class MovieBase: ComponentBase
	{
		[Inject]
		public IMovieService MovieService { get; set; }

		public IEnumerable<MovieDTO> Movies { get; set; }

		protected override async Task OnInitializedAsync()
		{
            Movies = await MovieService.GetItems();
		}
	}
}
