using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using CatalogAPI.Services;

namespace CatalogAPI.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<MovieService>();

            if (context.Get().Any())
            {
                return;
            }

            var genre = "Drama";
            var client = new RestClient("https://moviesdatabase.p.rapidapi.com/");
            var request = new RestRequest($"/titles?genre={genre}");
            request.AddHeader("X-RapidAPI-Key", "ee29f55c95mshe6cf2df54eb11e6p103991jsne76b8704cd9e");
            request.AddHeader("X-RapidAPI-Host", "moviesdatabase.p.rapidapi.com");

            var response = client.Get(request);

            if (response.IsSuccessful)
            {
                var content = JsonConvert.DeserializeObject<JToken>(response.Content);

                foreach (var movie in content["results"])
                {
                    if (movie != null)
                    {
                        string title = movie["titleText"]?["text"]?.Value<string>() ?? "No Title";
                        string description = movie["originalTitleText"]?["text"]?.Value<string>() ?? "No Description";
                        string pictureUri = null;
                        if (movie["primaryImage"]?.Type == JTokenType.Object)
                        {
                            pictureUri = movie["primaryImage"]?["url"]?.Value<string>() ?? "No Picture";
                        }
                        int releaseYear = movie["releaseYear"]?["year"]?.Value<int>() ?? 0;

                        DateTime? releaseDate = null;
                        var releaseDateToken = movie["releaseDate"];
                        if (releaseDateToken != null && releaseDateToken.Type == JTokenType.Object)
                        {
                            var day = releaseDateToken["day"]?.Value<int?>();
                            var month = releaseDateToken["month"]?.Value<int?>();
                            var year = releaseDateToken["year"]?.Value<int?>();

                            if (day.HasValue && month.HasValue && year.HasValue)
                            {
                                releaseDate = new DateTime(year.Value, month.Value, day.Value);
                            }
                        }

                        context.Create(
                            new Movie
                            {
                                Title = title,
                                Description = description,
                                PictureUri = pictureUri,
                                ReleaseYear = releaseYear,
                                ReleaseDate = releaseDate,
                                Price = 10.0m,
                                Stock = 100
                            }
                        );
                    }
                }
            }
            else
            {
                Console.WriteLine("Request failed: " + response.ErrorMessage);
            }
        }
    }
}