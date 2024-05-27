using SharedModels.Models;
using System.Net.Http.Json;

namespace NewWebApp.Services
{
    public class MovieService : IMovieService
    {
        private readonly HttpClient _httpClient;

        public MovieService(HttpClient httpClient)
        {
            _httpClient = httpClient;
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
