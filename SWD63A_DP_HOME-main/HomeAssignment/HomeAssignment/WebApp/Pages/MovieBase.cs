using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using SharedModels.Models;
using WebApp.Services;

namespace WebApp.Pages
{
    public class MovieBase : ComponentBase
    {
        [Inject]
        public IMovieService MovieService { get; set; }

        public IEnumerable<MovieDTO> Movies { get; set; }

        protected string Genre { get; set; } = "Action";

        protected string MovieName { get; set; }

        protected async Task LoadMoviesByGenre()
        {
            Movies = await MovieService.GetMoviesByGenre(Genre);
        }

        protected async Task LoadMoviesByName()
        {
            if (!string.IsNullOrEmpty(MovieName))
            {
                try
                {
                    Movies = await MovieService.GetMovieByName(MovieName);
                }
                catch (Exception ex)
                {
                    // Handle exception
                    Console.WriteLine($"Error loading movies by name: {ex.Message}");

                    // Reset the Movies property to null or an empty collection
                    Movies = null; // or Movies = Enumerable.Empty<MovieDTO>();
                }
            }
        }

        protected override async Task OnInitializedAsync()
        {
            await LoadMoviesByGenre();
        }
    }
}
