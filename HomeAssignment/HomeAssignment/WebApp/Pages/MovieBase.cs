using Microsoft.AspNetCore.Components;
using SharedModels.Models;
using WebApp.Services;

namespace WebApp.Pages
{
	public class MovieBase: ComponentBase
	{
		[Inject]
		public IMovieService MovieService { get; set; }

		public IEnumerable<MovieDTO> Movies { get; set; }
	}
}
