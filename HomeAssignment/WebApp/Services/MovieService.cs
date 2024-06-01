using SharedModels.Models;
using System.Net.Http.Json;

namespace WebApp.Services
{
    public class MovieService : IMovieService
    {
        private readonly HttpClient _httpClient;

        public MovieService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<MovieDTO>> GetMoviesByGenre(string genre)
        {
            try
            {
                var movies = await _httpClient.GetFromJsonAsync<IEnumerable<MovieDTO>>($"gateway/Movies/titles/movies/{genre}");
                return movies;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<MovieDTO>> GetMovieByName(string name)
        {
            try
            {
                var movie = await _httpClient.GetFromJsonAsync<IEnumerable<MovieDTO>>($"gateway/Movies/titles/movies/search/title/{name}");
                return movie;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<MovieDTO>> GetTVShowByGenre(string genre)
        {
            try
            {
                var movies = await _httpClient.GetFromJsonAsync<IEnumerable<MovieDTO>>($"gateway/Movies/titles/tv/{genre}");
                return movies;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<MovieDTO>> GetTVByName(string name)
        {
            try
            {
                var movie = await _httpClient.GetFromJsonAsync<IEnumerable<MovieDTO>>($"gateway/Movies/titles/tv/search/title/{name}");
                return movie;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<MovieDTO>> GetUpcoming()
        {
            try
            {
                var movie = await _httpClient.GetFromJsonAsync<IEnumerable<MovieDTO>>($"gateway/Movies/titles/x/upcoming");
                return movie;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task AddUpcoming(MovieDTO movie)
        {

            var response = await _httpClient.PostAsJsonAsync("gateway/Movies", movie);
            response.EnsureSuccessStatusCode();
        }

        public async Task<IEnumerable<MovieDTO>> GetItems()
        {
            try
            {
                var movies = await _httpClient.GetFromJsonAsync<IEnumerable<MovieDTO>>("gateway/Movies");
                return movies;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
