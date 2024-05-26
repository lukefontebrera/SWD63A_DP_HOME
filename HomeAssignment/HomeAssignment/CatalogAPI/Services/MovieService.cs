using CatalogAPI.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CatalogAPI.Services
{
    public class MovieService
    {
        private readonly IMongoCollection<Movie> _moviesCollection;

        public MovieService(
            IOptions<EShopStoreDatabaseSettings> movieDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                movieDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                movieDatabaseSettings.Value.DatabaseName);

            _moviesCollection = mongoDatabase.GetCollection<Movie>(
                movieDatabaseSettings.Value.CatalogCollectionName);
        }

        public async Task<List<Movie>> GetAsync() =>
            await _moviesCollection.Find(_ => true).ToListAsync();

        public List<Movie> Get() => _moviesCollection.Find(_ => true).ToList();

        public async Task<Movie?> GetAsync(string id) =>
            await _moviesCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Movie newMovie) =>
            await _moviesCollection.InsertOneAsync(newMovie);

        public void Create(Movie newMovie) => _moviesCollection.InsertOne(newMovie);

        public async Task UpdateAsync(string id, Movie updatedMovie) =>
            await _moviesCollection.ReplaceOneAsync(x => x.Id == id, updatedMovie);

        public async Task RemoveAsync(string id) =>
            await _moviesCollection.DeleteOneAsync(x => x.Id == id);
    }
}
