using CatalogAPI.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace CatalogAPI.Services
{
    public class MovieService
    {
        public List<Movie> GetMoviesByGenre(string genre)
        {
            var movies = new List<Movie>();

            try
            {
                var client = new RestClient("https://moviesdatabase.p.rapidapi.com/");
                var request = new RestRequest($"/titles?titleType=movie&genre={genre}");
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

                            movies.Add(new Movie
                            {
                                Title = title,
                                Description = description,
                                PictureUri = pictureUri,
                                ReleaseYear = releaseYear,
                                ReleaseDate = releaseDate,
                                Price = 10
                            });
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Request failed: " + response.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching movies: " + ex.Message);
            }

            return movies;
        }

        public List<Movie> GetSpecificMovie(string name)
        {
            var movies = new List<Movie>();

            try
            {
                var client = new RestClient("https://moviesdatabase.p.rapidapi.com/");
                var request = new RestRequest($"/titles/search/title/{name}");
                request.AddQueryParameter("titleType", "movie");
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
                            string pictureUri = movie["primaryImage"]?["url"]?.Value<string>() ?? "No Picture";
                            string captionPlainText = movie["caption"]?["plainText"]?.Value<string>() ?? "No Caption Plain Text";
                            string captionTypeName = movie["caption"]?["__typename"]?.Value<string>() ?? "No Caption Type Name";
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

                            movies.Add(new Movie
                            {
                                Title = title,
                                Description = description,
                                Caption = $"{captionPlainText} ({captionTypeName})",
                                PictureUri = pictureUri,
                                ReleaseYear = releaseYear,
                                ReleaseDate = releaseDate,
                                Price = 10
                            });
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Request failed: " + response.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching movies: " + ex.Message);
            }

            return movies;
        }

        public List<Movie> GetTVShowByGenre(string genre)
        {
            var movies = new List<Movie>();

            try
            {
                var client = new RestClient("https://moviesdatabase.p.rapidapi.com/");
                var request = new RestRequest($"/titles?titleType=tvSeries&genre={genre}");
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

                            movies.Add(new Movie
                            {
                                Title = title,
                                Description = description,
                                PictureUri = pictureUri,
                                ReleaseYear = releaseYear,
                                ReleaseDate = releaseDate,
                                Price = 10
                            });
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Request failed: " + response.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching movies: " + ex.Message);
            }

            return movies;
        }

        public List<Movie> GetSpecificTVShow(string name)
        {
            var movies = new List<Movie>();

            try
            {
                var client = new RestClient("https://moviesdatabase.p.rapidapi.com/");
                var request = new RestRequest($"/titles/search/title/{name}");
                request.AddQueryParameter("titleType", "tvSeries");
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
                            string pictureUri = movie["primaryImage"]?["url"]?.Value<string>() ?? "No Picture";
                            string captionPlainText = movie["caption"]?["plainText"]?.Value<string>() ?? "No Caption Plain Text";
                            string captionTypeName = movie["caption"]?["__typename"]?.Value<string>() ?? "No Caption Type Name";
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

                            movies.Add(new Movie
                            {
                                Title = title,
                                Description = description,
                                Caption = $"{captionPlainText} ({captionTypeName})",
                                PictureUri = pictureUri,
                                ReleaseYear = releaseYear,
                                ReleaseDate = releaseDate,
                                Price = 10
                            });
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Request failed: " + response.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching movies: " + ex.Message);
            }

            return movies;
        }
    }
}
