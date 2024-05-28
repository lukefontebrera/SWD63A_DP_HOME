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

        protected string Genre { get; set; }

        protected string MovieName { get; set; }

        protected async Task LoadMoviesByGenre()
        {
            if (!string.IsNullOrEmpty(Genre))
            {
                try
                {
                    Movies = await MovieService.GetMoviesByGenre(Genre);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error loading movies by genre: {ex.Message}");

                    Movies = null;
                }
            }
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
                    Console.WriteLine($"Error loading movies by name: {ex.Message}");

                    Movies = null;
                }
            }
        }

        protected async Task LoadTVByGenre()
        {
            if (!string.IsNullOrEmpty(Genre))
            {
                try
                {
                    Movies = await MovieService.GetTVShowByGenre(Genre);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error loading movies by genre: {ex.Message}");

                    Movies = null;
                }
            }
        }

        protected async Task LoadTVByName()
        {
            if (!string.IsNullOrEmpty(MovieName))
            {
                try
                {
                    Movies = await MovieService.GetTVByName(MovieName);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error loading movies by name: {ex.Message}");

                    Movies = null;
                }
            }
        }

        protected override async Task OnInitializedAsync()
        {
            await LoadMoviesByGenre();
        }
    }
}
