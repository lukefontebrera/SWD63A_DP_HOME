using SharedModels.Models;

namespace NewWebApp.Services
{
    public interface IMovieService
    {
        Task<IEnumerable<MovieDTO>> GetItems();
    }
}
