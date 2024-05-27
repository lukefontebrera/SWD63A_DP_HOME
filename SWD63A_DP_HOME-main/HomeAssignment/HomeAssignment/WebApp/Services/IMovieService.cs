using SharedModels.Models;

namespace WebApp.Services
{
    public interface IMovieService
    {
        Task<IEnumerable<MovieDTO>> GetMoviesByGenre(string genre);

        Task<IEnumerable<MovieDTO>> GetMovieByName(string name);
    }
}
