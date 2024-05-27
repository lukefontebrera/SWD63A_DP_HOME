using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using SharedModels.Models;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace WebApp.Services
{
    public class MovieService : IMovieService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey = "ee29f55c95mshe6cf2df54eb11e6p103991jsne76b8704cd9e";
        private readonly string _apiHost = "moviesdatabase.p.rapidapi.com";
        private readonly string _baseUrl = "https://moviesdatabase.p.rapidapi.com/";

        public MovieService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<MovieDTO>> GetMoviesByGenreAsync(string genre)
        {
            var client = new RestClient(_baseUrl);
            var request = new RestRequest("titles");
            request.AddHeader("X-RapidAPI-Key", _apiKey);
            request.AddHeader("X-RapidAPI-Host", _apiHost);

            if (!string.IsNullOrEmpty(genre))
            {
                request.AddParameter("genre", genre);
            }

            var response = await client.ExecuteGetAsync(request);

            if (response.IsSuccessful)
            {
                var content = JsonConvert.DeserializeObject<JToken>(response.Content);
                var movies = content["results"]
                    .Select(movie => new MovieDTO
                    {
                        Title = movie["titleText"]?["text"]?.Value<string>() ?? "No Title",
                        Description = movie["originalTitleText"]?["text"]?.Value<string>() ?? "No Description",
                        PictureUri = movie["primaryImage"]?["url"]?.Value<string>() ?? "No Picture",
                        ReleaseYear = movie["releaseYear"]?["year"]?.Value<int>() ?? 0,
                        ReleaseDate = movie["releaseDate"]?.Type == JTokenType.Object
                            ? new DateTime(
                                movie["releaseDate"]?["year"]?.Value<int>() ?? 0,
                                movie["releaseDate"]?["month"]?.Value<int>() ?? 1,
                                movie["releaseDate"]?["day"]?.Value<int>() ?? 1)
                            : (DateTime?)null
                    }).ToList();

                return movies;
            }
            else
            {
                throw new Exception($"Request failed: {response.ErrorMessage}");
            }
        }
    }

}
