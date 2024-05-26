using SharedModels.Models;

namespace WebApp.Services
{
    public interface IMovieService
    {
        Task<IEnumerable<MovieDTO>> GetItems();
    }
}
