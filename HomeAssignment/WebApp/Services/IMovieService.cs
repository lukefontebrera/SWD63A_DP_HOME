using SharedModels.Models;

namespace WebApp.Services
{
    public interface IMovieService
    {
        Task<IEnumerable<MovieDTO>> GetMoviesByGenre(string genre);

        Task<IEnumerable<MovieDTO>> GetMovieByName(string name);

        Task<IEnumerable<MovieDTO>> GetTVShowByGenre(string genre);

        Task<IEnumerable<MovieDTO>> GetTVByName(string name);

        Task<IEnumerable<MovieDTO>> GetUpcoming();

        Task<IEnumerable<MovieDTO>> GetItems();

        Task AddUpcoming(MovieDTO movie);
    }
}
